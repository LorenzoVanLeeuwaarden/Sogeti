import requests
import re
import MySQLdb

def connectDb():
    MySQLdb.connect(host="92.109.64.188", # your host, usually localhost
                     port=3306,
                     user="school", # your username
                      passwd="school", # your password
                      db="sogeti") # name of the data base