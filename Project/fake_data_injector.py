# import serial  # (시리얼 통신 안 함)
import mysql.connector # [수정] SQLite가 아닌 MySQL 드라이버
import time
import math
import random 

# [수정] MySQL 서버에 접속
db = mysql.connector.connect(
    host="localhost",
    user="root",
    password="1234",
    database="sensor_project"
)
cursor = db.cursor()

print("MySQL 가짜 데이터 생성기 시작... (Ctrl+C로 중지)")
print("아두이노 없이 MySQL에 1초마다 가짜 데이터를 삽입합니다.")

while True:
    try:
        # --- '가짜' 데이터를 여기서 직접 생성 ---
        co2 = random.randint(400, 600)               # 정상 범위
        temp = round(random.uniform(22.8, 23.2), 1) # 정상 범위
        hum = round(random.uniform(42.0, 48.0), 1)   # 정상 범위
        pm1 = round(random.uniform(1.0, 5.0), 1)   # (가짜 값)
        pm25 = round(random.uniform(5.0, 15.0), 1)   # 정상 범위 (C#이 이 컬럼을 봄)
        pm10 = round(random.uniform(10.0, 20.0), 1)  # (가짜 값)

        # --- 경고 테스트용 데이터 (C# 패널 빨갛게 되는지 보려면 이걸로 바꿔) ---
        # temp = 25.0  # 온도 경고
        # hum = 60.0   # 습도 경고
        # pm25 = 40.0 # 미세먼지 경고
        # co2 = 1200   # CO2 경고
        # ---

        # [수정] MySQL용 INSERT 쿼리 (소문자 컬럼, %s 포맷)
        sql = """INSERT INTO sensor_data
                 (co2_ppm, temperature, humidity, pm1_0, pm2_5, pm10)
                 VALUES (%s,%s,%s,%s,%s,%s)"""
        
        cursor.execute(sql, (co2, temp, hum, pm1, pm25, pm10))
        db.commit() # [수정] MySQL은 commit()이 필수

        # 로그 이름 변경 (아까 스크린샷에서 본 이름)
        print(f"데이터 삽입: T={temp}, H={hum}, Dust={pm25}, CO2={co2}")

        # 1초마다 실행
        time.sleep(1)

    except mysql.connector.Error as err:
        # [수정] MySQL 에러 처리
        print(f"MySQL 에러: {err}")
        # (만약 'Access Denied'가 뜬다면 ALTER USER... 쿼리 실행)
        time.sleep(1) 
    except Exception as e:
        print("기타 에러:", e)
        time.sleep(1)
    except KeyboardInterrupt:
        print("\n가짜 데이터 생성 중지.")
        break

cursor.close()
db.close()
