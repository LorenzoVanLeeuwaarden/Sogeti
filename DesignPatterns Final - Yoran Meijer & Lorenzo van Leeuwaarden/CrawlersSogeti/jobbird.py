from httplib import HTTP
import httplib
import string
from urllib import urlopen
import urllib
from BeautifulSoup import BeautifulSoup
import urlparse
import mechanize
import requests
import re
import MySQLdb
import cleaner
# -*- coding: utf-8 -*-

def sendToDb():

        db = MySQLdb.connect(host="92.109.64.188", # your host, usually localhost
                     port=3306,
                     user="school", # your username
                      passwd="school", # your password
                      db="sogeti") # name of the data base
        sql = ""

        cur=db.cursor()
        idSql = "SELECT bedrijfsID FROM sogeti.bedrijf WHERE bedrijfsnaam = '"+dbBedrijf+"'"
        bedrijfCount=cur.execute(idSql)

        while bedrijfCount == 0:
            #insert bedrijf en check opnieuw om bedrijfid op te halen...
            sql = "INSERT INTO sogeti.bedrijf(bedrijfsnaam, adres, postcode, plaats) VALUES ('"+dbBedrijf+"','"+dbStraat+"','"+dbPostcode+"','"+dbLocatie+"')"
            cur.execute(sql)
            db.commit()

            idSql = "SELECT bedrijfsID FROM sogeti.bedrijf WHERE bedrijfsnaam = '"+dbBedrijf+"'"
            bedrijfCount=cur.execute(idSql)

        if bedrijfCount != 0:
            for bedrijfsID in cur:
                dbID=bedrijfsID

            dbID = str(dbID[0])

            vacatureCheck = "SELECT vacatureID FROM sogeti.vacature WHERE bedrijfsID = '"+dbID+"' AND omschrijving = '"+dbOmschrijving+"'"
            vacatureCheck = cur.execute(vacatureCheck)
            vCheck=0
            for vacatureID in cur:
                vCheck = vCheck+1

            print vCheck
            cur.execute("set names utf8;")
            if dbID:
                if vCheck==0:
                    sql = "INSERT INTO sogeti.vacature(bedrijfsID,opleiding,baantype,omschrijving,locatie) VALUES ('"+dbID+"','"+dbOpleiding+"','"+dbType+"','"+dbOmschrijving+"','"+dbLocatie+"')"
                    cur.execute(sql)
                    db.commit()
        db.close()
        cur.close()
        return


br = mechanize.Browser()
goodurls = []

for x in range(100, 520):


    url = "http://www.jobbird.com/nl/kandidaat/vacature-zoekresultaat?page="+unicode(x)+"&search=ict"

    r = requests.get(url)

    data = r.text

    soup = BeautifulSoup(data)

    for link in soup.findAll('a', href=re.compile('/nl/vacature/(\d*?)/(.*?)')):

        goodurls.append(link.get('href'))




for url in goodurls:
    print "http://www.jobbird.com"+url

    webpage = urlopen("http://www.jobbird.com"+url).read()
    soup2 = BeautifulSoup(webpage)

    findBedrijf = ""
    findStraat = ""
    findPostcode = ""
    findLocatie = ""
    findBranche = ""
    findType = ""
    findOpleiding = ""
    findOmschrijving = ""
    dbOmschrijving = ""

    patFindBedrijfsinfo = re.compile('<div id="content-contact">(.*?)<br>(.*?)<br>(.*?)&')
    bedrijfsinfo = re.findall(patFindBedrijfsinfo, webpage)
    if bedrijfsinfo:
        findBedrijf = bedrijfsinfo.__getitem__(0)[0]
        findStraat = bedrijfsinfo.__getitem__(0)[1]
        findPostcode = bedrijfsinfo.__getitem__(0)[2]
        print findBedrijf
        print findStraat
        print findPostcode

    patFindLocatie = re.compile('<div class="BoldFont">Locatie</div>(.*?)</p>')
    getLocatie = re.findall(patFindLocatie, webpage)
    if getLocatie:
        findLocatie = getLocatie.__getitem__(0)
        print findLocatie

    patFindBranche = re.compile('<div class="BoldFont">Branche</div><div class="field-item"><a href="(.*?)">(.*?)</a>')
    getBranche = re.findall(patFindBranche, webpage)
    if getBranche:
        findBranche = getBranche.__getitem__(0)[1]
        print findBranche

    patFindType = re.compile('<div class="BoldFont">Dienstverband</div><div class="field-item">(.*?)</div>')
    getType = re.findall(patFindType, webpage)
    if getType:
        findType = getType.__getitem__(0)
        print findType

    patFindOpleiding = re.compile('<div class="BoldFont">Opleidingsniveau</div><div class="field-item">(.*?)</div>')
    getOpleiding = re.findall(patFindOpleiding, webpage)
    if getOpleiding:
        findOpleiding = getOpleiding.__getitem__(0)
        print findOpleiding

    patFindOmschrijving = re.compile('<div class="vacatureSubtitels">(.*?)<div', re.DOTALL)
    findOmschrijving = re.findall(patFindOmschrijving, webpage)
    if findOmschrijving:
        findOmschrijving = findOmschrijving.__getitem__(0)
        dbOmschrijving = str(findOmschrijving)
        dbOmschrijving = cleaner.clearTags(dbOmschrijving)
        dbOmschrijving = cleaner.cleanChars(dbOmschrijving)

    try:
        dbOmschrijving.decode("UTF-8")
    except:
        print "dan niet joh"


    dbBedrijf = cleaner.clearTags(findBedrijf)
    dbStraat = cleaner.clearTags(findStraat)
    dbPostcode = cleaner.clearTags(findPostcode)
    dbLocatie = cleaner.clearTags(findLocatie)
    dbBranche = cleaner.clearTags(findBranche)
    dbType = cleaner.clearTags(findType)
    dbOpleiding = cleaner.clearTags(findOpleiding)
    badOmschrijving = False


    dbBedrijf = cleaner.cleanChars(findBedrijf)
    dbStraat = cleaner.cleanChars(findStraat)
    dbPostcode = cleaner.cleanChars(findPostcode)
    dbLocatie = cleaner.cleanChars(findLocatie)
    dbBranche = cleaner.cleanChars(findBranche)
    dbType = cleaner.cleanChars(findType)
    dbOpleiding = cleaner.cleanChars(findOpleiding)

    if not dbOmschrijving:
        badOmschrijving = True

    print dbBedrijf
    #print dbLocatie
    #print dbBranche
    #print dbType
    #print dbErvaring
    #print dbOpleiding
    #print dbCarriere
    print dbOmschrijving


    if badOmschrijving != True:
        sendToDb()

