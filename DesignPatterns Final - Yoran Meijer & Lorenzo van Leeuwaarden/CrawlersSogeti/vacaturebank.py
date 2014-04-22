# -*- coding: utf-8 -*-
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

#Werkervaring niet op de site bekend, wordt dus leeggelaten

def sendToDb():

        db = MySQLdb.connect(host="localhost", # your host, usually localhost
                     user="root", # your username
                      passwd="", # your password
                      db="sogeti") # name of the data base
        sql = ""

        #findErvaring = findErvaring.replace("'","")


        cur=db.cursor()
        idSql = "SELECT bedrijfsID FROM sogeti.bedrijf WHERE bedrijfsnaam = '"+dbBedrijf+"'"
        bedrijfCount=cur.execute(idSql)

        while bedrijfCount == 0:
            #insert bedrijf en check opnieuw om bedrijfid op te halen...
            sql = "INSERT INTO sogeti.bedrijf(bedrijfsnaam) VALUES ('"+dbBedrijf+"')"
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

            if dbID:
                if vCheck==0:
                    try:
                        sql = "INSERT INTO sogeti.vacature(bedrijfsID,opleiding,baantype,carriereniveau,omschrijving,locatie) VALUES ('"+dbID+"','"+dbOpleiding+"','"+dbType+"','"+dbCarriere+"','"+dbOmschrijving+"','"+dbLocatie+"')"
                        cur.execute(sql)
                        db.commit()
                    except:
                        return
        return

br = mechanize.Browser()
goodurls = []

for x in range(1, 2):


    url = "http://www.nationalevacaturebank.nl/vacature/zoeken/overzicht/relevant/page/"+unicode(x)+"/query/ICT/distance/30/output/html/load_mode/results_page/items_per_page/50/ignore_ids//fetchDistances"

    r = requests.get(url)

    data = r.text

    soup = BeautifulSoup(data)

    for link in soup.findAll('a', href=re.compile('http://www.nationalevacaturebank.nl/vacature/(\d*?)/')):
        if link.get('href') not in goodurls:
            goodurls.append(link.get('href'))




for url in goodurls:
    print url

    webpage = urlopen(url).read()

    soup2 = BeautifulSoup(webpage)
    a = soup2.find("meta", {"name":"title"})['content']

    bedrijfsInfo = re.compile('(.*?) \| (.*?) \| (.*?) \| (\d*)')
    findInformatie = re.findall(bedrijfsInfo, a)
    if findInformatie:
        if findInformatie.__getitem__(0)[3]:
            vacatureTitel = unicode(findInformatie.__getitem__(0)[0])
            bedrijfsNaam = unicode(findInformatie.__getitem__(0)[1])
            bedrijfsPlaats = unicode(findInformatie.__getitem__(0)[2])
            print vacatureTitel

        else:
            bedrijfsInfo = re.compile('(.*?) \| (.*?) \| (.*?) \| (.*?) \|')
            findInformatie = re.findall(bedrijfsInfo, a)
            vacatureTitel = unicode(findInformatie.__getitem__(0)[0])
            bedrijfsNaam = unicode(findInformatie.__getitem__(0)[2])
            bedrijfsPlaats = unicode(findInformatie.__getitem__(0)[3])
            print vacatureTitel


    dbBedrijf = cleaner.clearTags(bedrijfsNaam)
    dbLocatie = cleaner.clearTags(bedrijfsPlaats)
    dbBedrijf = cleaner.cleanChars(dbBedrijf)
    dbLocatie = cleaner.cleanChars(dbLocatie)


    print dbBedrijf
    print dbLocatie


    branche = re.compile('<dt>Branche:</dt>(\s*?)<dd>(\s*?)<a href="(.*?)" class="non-bold">(.*?)</a><br />(\s*?)<a href="(.*?)" class="non-bold">(.*?)</a>')
    findBranche = re.findall(branche, webpage)
    if findBranche:
        Branche1 = findBranche.__getitem__(0)[3]
        Branche2 = findBranche.__getitem__(0)[6]
        findBranche = Branche1 + ", " + Branche2



    if not findBranche:
        branche = re.compile('<dt>Branche:</dt>(\s*?)<dd>(\s*?)<a href="(.*?)" class="non-bold">(.*?)</a>')
        findBranche = re.findall(branche, webpage)
        if findBranche:
            findBranche = findBranche.__getitem__(0)[3]

    findBranche = unicode(findBranche)
    dbBranche = cleaner.clearTags(findBranche)
    dbBranche = cleaner.cleanChars(findBranche)
    print findBranche

    type = re.compile('<dt>Dienstverband:</dt>(\s*?)<dd>(.*?)</dd>')
    findType = re.findall(type, webpage)
    if findType:
        findType = findType.__getitem__(0)[1]
        findType = unicode(findType)
        dbType = cleaner.clearTags(findType)
        dbType = cleaner.cleanChars(findType)
        print findType

    opleiding = re.compile('<dt>Opleidingsniveau:</dt>(\s*?)<dd>(.*?)</dd>')
    findOpleiding = re.findall(opleiding, webpage)
    if findOpleiding:
        findOpleiding = unicode(findOpleiding.__getitem__(0)[1])
        findOpleiding = unicode(findOpleiding)
        dbOpleiding = cleaner.clearTags(findOpleiding)
        dbOpleiding = cleaner.cleanChars(findOpleiding)
        print findOpleiding

    carriere = re.compile('<dt>Carri&egrave;reniveau:</dt>(\s*?)<dd>(.*?)</dd>')
    findCarriere = re.findall(carriere, webpage)
    if findCarriere:
        findCarriere = findCarriere.__getitem__(0)[1]
        findCarriere = unicode(findCarriere)
        dbCarriere = cleaner.clearTags(findCarriere)
        dbCarriere = cleaner.cleanChars(findCarriere)
        print findCarriere

    findOmschrijving = soup2.find('div', attrs={'class':'body'})
    badOmschrijving = False
    #if not findOmschrijving:
        #patFindOmschrijving = re.compile('id="vacature-detail-view">(.*?)</div>', re.DOTALL)
        #findOmschrijving = re.findall(patFindOmschrijving, webpage)
        #try:
            #findOmschrijving = findOmschrijving.__getitem__(0).encode('utf-8','ignore')
        #except:
            #badOmschrijving = True


    newOmschrijving = unicode(findOmschrijving)
    dbOmschrijving = cleaner.clearTags(newOmschrijving)
    dbOmschrijving = cleaner.cleanChars(dbOmschrijving)

    print dbOmschrijving
    if badOmschrijving != True:
        sendToDb()





