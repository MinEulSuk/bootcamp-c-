# import serial # (시리얼 통신 주석 처리됨)
import mysql.connector # [수정] SQLite가 아닌 MySQL 드라이버
import time
import math
import random  # 가짜 데이터 만들 'random' 모듈

# ser = serial.Serial("COM8", 115200, timeout=1) # (아두이노 주석 처리됨)

# DB 연결 정보 (조원이 준 것과 동일)
db = mysql.connector.connect(
    host="localhost",
    user="root",
    password="1234",
    database="sensor_project"
)
cursor = db.cursor()

# start_time = time.time()  # (워밍업 로직 주석 처리됨)
# WARMUP_TIME = 5

def safe_float(val):
    try:
        f = float(val)
        if math.isnan(f) or math.isinf(f):
            return None
        return f
    except:
        return None

print("MySQL *간헐적 경고* 데이터 생성기 시작... (Ctrl+C로 중지)")
print("C# 경고/정상 복구 테스트를 위해 데이터를 50% 확률로 섞어 삽입합니다.")

while True:
    
    try:
        # --- [수정] 50% 확률로 정상/경고 데이터를 섞어서 생성 ---

        # 1. 온도 (C# 기준: 22.5~23.5)
        if random.random() < 0.5: # 50% 확률
            temp = round(random.uniform(24.0, 26.0), 1) # 경고 (고온)
        else:
            temp = round(random.uniform(22.8, 23.2), 1) # 정상

        # 2. 습도 (C# 기준: 40~50)
        if random.random() < 0.5: # 50% 확률
            hum = round(random.uniform(55.0, 65.0), 1) # 경고 (고습)
        else:
            hum = round(random.uniform(42.0, 48.0), 1) # 정상

        # 3. 미세먼지 (C# 기준: 35 미만)
        if random.random() < 0.5: # 50% 확률
            pm25 = round(random.uniform(35.0, 45.0), 1) # 경고
        else:
            pm25 = round(random.uniform(5.0, 15.0), 1) # 정상

        # 4. CO2 (C# 기준: 1000 미만)
        if random.random() < 0.5: # 50% 확률
            co2 = random.randint(1100, 1300) # 경고
        else:
            co2 = random.randint(400, 600) # 정상

        # (다른 값들은 경고와 무관하므로 대충 설정)
        pm1 = round(pm25 / 3.0, 1) 
        pm10 = round(pm25 * 1.2, 1)
        # ---


        # MySQL에 삽입하는 로직 (그대로)
        sql = """INSERT INTO sensor_data
                 (co2_ppm, temperature, humidity, pm1_0, pm2_5, pm10)
                 VALUES (%s,%s,%s,%s,%s,%s)"""
        
        cursor.execute(sql, (co2, temp, hum, pm1, pm25, pm10))
        db.commit()

        print(f"[간헐적 테스트] 데이터 삽입: T={temp}, H={hum}, Dust={pm25}, CO2={co2}")

        # 1초마다 실행
        time.sleep(1)

    except Exception as e:
        print("Parse error or DB error:", e)
        time.sleep(1) # 에러가 나도 1초는 쉬고 다음 루프 실행

