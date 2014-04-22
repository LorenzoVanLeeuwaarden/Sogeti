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

        db = MySQLdb.connect(host="localhost", # your host, usually localhost
                     user="root", # your username
                      passwd="", # your password
                      db="sogeti") # name of the data base
        sql = ""

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
                    sql = "INSERT INTO sogeti.vacature(bedrijfsID,werkervaring,opleiding,baantype,carriereniveau,omschrijving,locatie) VALUES ('"+dbID+"','"+dbErvaring+"','"+dbOpleiding+"','"+dbType+"','"+dbCarriere+"','"+dbOmschrijving+"','"+dbLocatie+"')"
                    cur.execute(sql)
                    db.commit()
        return


br = mechanize.Browser()
goodurls = []

for x in range(1, 3):


    url = "http://jobsearch.monsterboard.nl/vacatures/?q=ICT&pg="+str(x)+"&cy=nl"

    r = requests.get(url)

    data = r.text

    soup = BeautifulSoup(data)

    for link in soup.findAll('a', href=re.compile('http://vacature.monsterboard.nl(.*)')):

        goodurls.append(link.get('href'))




for url in goodurls:
    print url

    webpage = urlopen(url).read()
    soup2 = BeautifulSoup(webpage)

    patFindBedrijf = re.compile("TrackingCompany: '(.*?)'")
    findBedrijf = re.findall(patFindBedrijf, webpage)
    if findBedrijf:

        dbBedrijf = str(findBedrijf.__getitem__(0))

    patFindLocatie = re.compile('<span class="wrappable" itemprop="jobLocation">(.*?)</span>')
    findLocatie = re.findall(patFindLocatie, webpage)
    if findLocatie:
        dbLocatie = str(findLocatie.__getitem__(0))

    if not findLocatie:
        patFindLocatie2 = re.compile('<span itemprop="jobLocation">(.*?)</span>')
        findLocatie = re.findall(patFindLocatie2, webpage)
        if findLocatie:
            dbLocatie = str(findLocatie.__getitem__(0))

    if not findLocatie:
        patFindLocatie3 = re.compile('itemprop="jobLocation"><span itemprop="name">(.*?)</span>')
        findLocatie = re.findall(patFindLocatie3, webpage)
        if findLocatie:
            dbLocatie = str(findLocatie.__getitem__(0))

    if not findLocatie:
        patFindLocatie4 = re.compile('name="jobLocation" value="(.*?)">')
        findLocatie = re.findall(patFindLocatie4, webpage)
        if findLocatie:
            dbLocatie = str(findLocatie.__getitem__(0))

    if not findLocatie:
        patFindLocatie5 = re.compile('itemprop="joblocation" content="(.*?)"/>')
        findLocatie = re.findall(patFindLocatie5, webpage)
        if findLocatie:
            dbLocatie = str(findLocatie.__getitem__(0))


    patFindBranche = re.compile('<span class="wrappable" itemprop="industry">(.*?)</span></dd><dd class="multipledd"><span class="wrappable">(.*?)</span></dd><dd class="multipleddlast"><span class="wrappable">(.*?)</span></dd>')
    findBranche = re.findall(patFindBranche, webpage)
    if findBranche:
        dbBranche = str(findBranche.__getitem__(0))

    if not findBranche:
        patFindBranche2 = re.compile('itemprop="occupationalCategory">(.*?)</span>')
        findBranche = re.findall(patFindBranche2, webpage)
        if findBranche:
            dbBranche = str(findBranche.__getitem__(0))


    if not findBranche:
        patFindBranche3 = re.compile('<span class="wrappable" itemprop="industry">(.*?)</span>')
        findBranche = re.findall(patFindBranche3, webpage)
        if findBranche:
            dbBranche = str(findBranche.__getitem__(0))

    if not findBranche:
        patFindBranche4 = re.compile('itemprop="industry">(.*?)</span></dd><dd class="multipleddlast"><span class="wrappable">(.*?)</span>')

    if not findBranche:
        patFindBranche5 = re.compile('itemprop="occupationalCategory">(.*?)</span>')
        findBranche = re.findall(patFindBranche5, webpage)
        if findBranche:
            dbBranche = str(findBranche.__getitem__(0))

    if not findBranche:
        patFindBranche6 = re.compile('<strong>Job Category:</strong>(.*?)<br />')
        findBranche = re.findall(patFindBranche6, webpage)
        if findBranche:
            dbBranche = str(findBranche.__getitem__(0))

    if not findBranche:
        findBranche = "ICT"
        dbBranche = findBranche

    patFindType = re.compile('itemprop="employmentType">(.*?)</span></dd><dd class="multipleddlast"><span class="wrappable">(.*?)</span>')
    findType = re.findall(patFindType, webpage)
    if findType:
        dbType = str(findType.__getitem__(0))

    if not findType:
        patFindType2 = re.compile('itemprop="employmentType">(.*?)</span>')
        findType = re.findall(patFindType2, webpage)
        if findType:
            dbType = str(findType.__getitem__(0))


    if not findType:
        patFindType3 = re.compile('<td class="bold">Status:</td><td><span class="wrappable">(.*?)</span>')
        findType = re.findall(patFindType3, webpage)
        if findType:
            dbType = str(findType.__getitem__(0))

    if not findType:
        findType = str("Niet bekend")
        dbType = findType

    patFindErvaring = re.compile('itemprop="experienceRequirements">(.*?)</span>')
    findErvaring = re.findall(patFindErvaring, webpage)
    if findErvaring:
            dbErvaring = str(findErvaring.__getitem__(0))

    if not findErvaring:
        patFindErvaring2 = re.compile('Werkervaring: (.*?)<br>')
        findErvaring = re.findall(patFindErvaring2, webpage)
        if findErvaring:
            dbErvaring = str(findErvaring.__getitem__(0))

    if not findErvaring:
        lists = soup2.findAll('li')
        for li in lists:
            patFindErvaring3 = re.compile('<li>(.*?)(ervaring)(.*?)</li>')
            findlist = re.findall(patFindErvaring3, str(li))

            if findlist:
                a = findlist.__getitem__(0)[0]
                b = findlist.__getitem__(0)[1]
                c = findlist.__getitem__(0)[2]
                d = a+b+c
                findErvaring = d
                dbErvaring = findErvaring

    if not findErvaring:
        findErvaring = str("Niet vereist")
        dbErvaring = findErvaring

    patFindOpleiding = re.compile('itemprop="educationRequirements">(.*?)<br><br></span>')
    findOpleiding = re.findall(patFindOpleiding, webpage)
    if findOpleiding:
            dbOpleiding = str(findOpleiding.__getitem__(0))

    if not findOpleiding:
        patFindOpleiding = re.compile('itemprop="educationRequirements">(.*?)<br></span>')
        findOpleiding = re.findall(patFindOpleiding, webpage)
        if findOpleiding:
            dbOpleiding = str(findOpleiding.__getitem__(0))

    if not findOpleiding:
        patFindOpleiding = re.compile('itemprop="educationRequirements">(.*?)</span>')
        findOpleiding = re.findall(patFindOpleiding, webpage)
        if findOpleiding:
            dbOpleiding = str(findOpleiding.__getitem__(0))

    if not findOpleiding:
        patFindOpleiding2 = re.compile('Opleiding: (.*?)<br>')
        findOpleiding = re.findall(patFindOpleiding2, webpage)
        if findOpleiding:
            dbOpleiding = str(findOpleiding.__getitem__(0))

    if not findOpleiding:
        lists = soup2.findAll('li')
        for li in lists:
            patFindOpleiding3 = re.compile('<li>(.*?)(HBO|MBO|Universitair|Dimploma|WO|VWO|HAVO|MAVO)(.*?)</li>')
            findlist = re.findall(patFindOpleiding3, str(li))

            if findlist:
                a = findlist.__getitem__(0)[0]
                b = findlist.__getitem__(0)[1]
                c = findlist.__getitem__(0)[2]
                d = a+b+c
                findOpleiding = d
                dbOpleiding = findOpleiding

    if not findOpleiding:
        findOpleiding = str("niet gespecificeerd")
        dbOpleiding = findOpleiding

    patFindCarriere = re.compile('itemprop="qualifications">(.*?)</span>')
    findCarriere = re.findall(patFindCarriere, webpage)
    if findCarriere:
            dbCarriere = str(findCarriere.__getitem__(0))

    if not findCarriere:
        findCarriere = str("niet gespecificeerd")
        dbCarriere = findCarriere

    patFindOmschrijving = re.compile("NAME='TrackingJobBody'>(.*?)</span>", re.DOTALL)

    findOmschrijving = re.findall(patFindOmschrijving, webpage)

    if findOmschrijving:
            dbOmschrijving = str(findOmschrijving.__getitem__(0))


    dbBedrijf = cleaner.clearTags(dbBedrijf)
    dbLocatie = cleaner.clearTags(dbLocatie)
    dbBranche = cleaner.clearTags(dbBranche)
    dbType = cleaner.clearTags(dbType)
    dbErvaring = cleaner.clearTags(dbErvaring)
    dbOpleiding = cleaner.clearTags(dbOpleiding)
    dbCarriere = cleaner.clearTags(dbCarriere)
    dbOmschrijving = cleaner.clearTags(dbOmschrijving)
    badOmschrijving = False


    #try:
        #dbOmschrijving = dbOmschrijving.encode('string-escape')
        #dbOmschrijving = dbOmschrijving.encode('utf-8','ignore')
    #except:
        #badOmschrijving = True

    dbBedrijf = cleaner.cleanChars(dbBedrijf)
    dbLocatie = cleaner.cleanChars(dbLocatie)
    dbBranche = cleaner.cleanChars(dbBranche)
    dbType = cleaner.cleanChars(dbType)
    dbErvaring = cleaner.cleanChars(dbErvaring)
    dbOpleiding = cleaner.cleanChars(dbOpleiding)
    dbCarriere = cleaner.cleanChars(dbCarriere)
    dbOmschrijving = cleaner.cleanChars(dbOmschrijving)


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

