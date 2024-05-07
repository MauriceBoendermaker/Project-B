import json
import random

cinema_data = {
    "cinema_rooms": [
        {
            "room_number": "Room 1",
            "seating": [
                [{"seat_number": "A1", "status": True, "type": "normal", "price": 10.0}, 
                 {"seat_number": "A2", "status": True, "type": "normal", "price": 10.0}],
                [{"seat_number": "B1", "status": True, "type": "normal", "price": 10.0}, 
                 {"seat_number": "B2", "status": True, "type": "normal", "price": 10.0}]
            ],
            "schedule": {
                "2024-04-01": [
                    {"movie": "The Shawshank Redemption", "start_time": "09:00", "price_multiplier": 1.0},
                    {"movie": "Inception", "start_time": "09:15", "price_multiplier": 1.0}
                ],
                "2024-04-02": [
                    {"movie": "Inception", "start_time": "09:00", "price_multiplier": 1.0},
                    {"movie": "The Shawshank Redemption", "start_time": "09:15", "price_multiplier": 1.0}
                ],
            }
        }
    ]
}


def generate_seat_number(row, col):
    return chr(65 + row) + str(col + 1)


for room_number in range(1, 6):
    rows = random.choice([8, 9, 10])
    seating = []
    for row in range(rows):
        seating_row = []
        cols = rows
        for col in range(cols):
            if row == 0 and (col < 3 or col >= cols - 3):
                seat_type = "handicap"
                price = 10.0
            elif row % 2 == 0 and (col < 2 or col >= cols - 2):
                seat_type = "love seat"
                price = 20.0
            else:
                seat_type = "normal"
                price = 10.0

            seat = {
                "seat_number": generate_seat_number(row, col), 
                "status": True, 
                "type": seat_type, 
                "price": price
            }
            seating_row.append(seat)
        seating.append(seating_row)

    cinema_data["cinema_rooms"].append({
        "room_number": f"Room {room_number}",
        "seating": seating,
        "schedule": {
            "2024-04-01": [
                {"movie": "The Shawshank Redemption", "start_time": "09:00", "price_multiplier": 1.0},
                {"movie": "Inception", "start_time": "09:15", "price_multiplier": 1.0}
            ],
            "2024-04-02": [
                {"movie": "Inception", "start_time": "09:00", "price_multiplier": 1.0},
                {"movie": "The Shawshank Redemption", "start_time": "09:15", "price_multiplier": 1.0}
            ],
        }
    })

modified_json = json.dumps(cinema_data, indent=2)
print(modified_json)