-- migrate main #0 to #1
CREATE TABLE IF NOT EXISTS people
(
    ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    NAME TEXT
)