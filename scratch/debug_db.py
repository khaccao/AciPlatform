import pyodbc
conn_str = 'DRIVER={ODBC Driver 17 for SQL Server};SERVER=103.200.22.167;DATABASE=Perfectkey_V1_Clean;UID=cao_admin;PWD=Aci12345678@'
try:
    conn = pyodbc.connect(conn_str)
    cursor = conn.cursor()
    print("Checking HotelCode 'HOMEHG'...")
    cursor.execute("SELECT COUNT(*) FROM PmsRooms WHERE HotelCode = 'HOMEHG'")
    count = cursor.fetchone()[0]
    print(f"PmsRooms count for HOMEHG: {count}")

    print("\nDistinct HotelCodes in PmsRooms:")
    cursor.execute("SELECT DISTINCT HotelCode FROM PmsRooms")
    for row in cursor.fetchall():
        print(f"- {row[0]}")

    print("\nRoom Types for HOMEHG:")
    cursor.execute("SELECT Loai_phong, COUNT(*) FROM PmsRooms WHERE HotelCode = 'HOMEHG' GROUP BY Loai_phong")
    for row in cursor.fetchall():
        print(f"- {row[0]}: {row[1]}")

    print("\nSP Definition for SP_GetRoomForecast:")
    cursor.execute("SELECT definition FROM sys.sql_modules WHERE object_id = OBJECT_ID('SP_GetRoomForecast')")
    row = cursor.fetchone()
    if row:
        print(row[0])
    else:
        print("SP not found!")

except Exception as e:
    print(f"Error: {e}")
