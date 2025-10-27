import sqlite3
import time
import random

DB_FILE = 'sensor_database.db'

def create_table(conn):
    """sensor_data 테이블이 없으면 생성"""
    try:
        cursor = conn.cursor()
        cursor.execute("""
        CREATE TABLE IF NOT EXISTS sensor_data (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
          Temperature REAL,
          Humidity REAL,
          Pm1_0 REAL,
          Pm2_5 REAL,
          Pm10 REAL,
          Co2Ppm INTEGER
        );
        """)
        
        # [중요] commands 테이블도 없으면 여기서 같이 만들어주자.
        cursor.execute("""
        CREATE TABLE IF NOT EXISTS commands (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          command_text TEXT NOT NULL,
          processed INTEGER DEFAULT 0,
          timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
        );
        """)
        
        conn.commit()
        print("DB 테이블 확인/생성 완료.")
    except sqlite3.Error as e:
        print(f"DB 테이블 생성 실패: {e}")
    finally:
        cursor.close()

def insert_fake_data(conn):
    """DB에 가짜 센서 데이터를 한 줄 삽입"""
    
    # --- 여기서 가짜 데이터 생성 ---
    # 경고 테스트하려면 이 범위를 벗어나게 값을 넣어봐
    fake_temp = round(random.uniform(22.8, 23.2), 1) # 정상 범위
    fake_hum = round(random.uniform(42.0, 48.0), 1)   # 정상 범위
    fake_pm2_5 = round(random.uniform(5.0, 15.0), 1)   # 정상 범위
    fake_co2 = random.randint(400, 600)               # 정상 범위

    # --- 경고 발생용 가짜 데이터 (이걸로 테스트해봐) ---
    # fake_temp = 25.0  # 온도 경고
    # fake_hum = 60.0   # 습도 경고
    # fake_pm2_5 = 40.0 # 미세먼지 경고
    # fake_co2 = 1200   # CO2 경고
    
    sql = """
    INSERT INTO sensor_data (Temperature, Humidity, Pm2_5, Co2Ppm)
    VALUES (?, ?, ?, ?)
    """
    
    try:
        cursor = conn.cursor()
        cursor.execute(sql, (fake_temp, fake_hum, fake_pm2_5, fake_co2))
        conn.commit()
        print(f"데이터 삽입: T={fake_temp}, H={fake_hum}, Dust={fake_pm2_5}, CO2={fake_co2}")
    except sqlite3.Error as e:
        print(f"DB 에러: {e}") # 여기서 'file is not a database'가 떴던 것
    finally:
        cursor.close()

def main():
    print("가짜 데이터 생성기 시작... (Ctrl+C로 중지)")
    
    try:
        conn = sqlite3.connect(DB_FILE)
    except sqlite3.Error as e:
        print(f"DB 연결 실패: {e}")
        return

    # --- [수정] ---
    # 루프 시작 전에 테이블부터 확실하게 생성
    create_table(conn)
    # ---------------

    while True:
        try:
            insert_fake_data(conn)
            time.sleep(1) # C# 타이머와 동일하게 1초 대기
        except KeyboardInterrupt:
            print("\n가짜 데이터 생성 중지.")
            break
    
    conn.close()

if __name__ == "__main__":
    main()