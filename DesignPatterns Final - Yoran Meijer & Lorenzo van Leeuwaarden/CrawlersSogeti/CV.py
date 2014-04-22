from urllib import urlopen
from BeautifulSoup import BeautifulSoup
import requests
import re

goodusernames = []
goodurls = []

for x in range(1, 3):


    url = "https://www.elance.com/r/contractors/cat-it-programming/ind-true/p-"+str(x)

    r = requests.get(url)

    data = r.text

    soup = BeautifulSoup(data)

    for link in soup.findAll('a', href=re.compile('https://www.elance.com/s/(.*?)/')):
        var = re.findall('https://www.elance.com/s/(.*?)/', str(link))
        username = var.__getitem__(0)
        if username not in goodusernames:
            goodusernames.append(username)
            url = 'https://www.elance.com/s/'
            goodurls.append(url + username + '/')

for url in goodurls:
    print url
    webpage = urlopen(url).read()
    cvpage = urlopen(url+"resume/").read()

    badCV = False

    patFindName = re.compile('id="cta-title">(.*?)</h1>')
    findName = re.findall(patFindName, webpage)
    if findName:
        findName = findName.__getitem__(0)
    if not findName:
        badCV = True

    patFindNationaliteit = re.compile('class="country-name">(.*?)</span>')
    findNationaliteit = re.findall(patFindNationaliteit, webpage)
    if findNationaliteit:
        findNationaliteit = findNationaliteit.__getitem__(0)

    patFindCity = re.compile('class="city">(.*?)</span>')
    findCity = re.findall(patFindCity, webpage)
    if findCity:
        findCity = findCity.__getitem__(0)
    if not findCity:
        findCity = "Not Found"

    patFindOmschrijving = re.compile('class="p-about-txt">(.*?)</div>', re.DOTALL)
    findOmschrijving = re.findall(patFindOmschrijving, cvpage)
    if findOmschrijving:
        findOmschrijving = findOmschrijving.__getitem__(0)
        findOmschrijving = findOmschrijving.replace("=&gt;", "- ")
        findOmschrijving = findOmschrijving.replace("==&gt;", "- ")
        findOmschrijving = findOmschrijving.replace("&gt;", " - ")
        findOmschrijving = findOmschrijving.replace("<br />", "\n")
        findOmschrijving = findOmschrijving.replace("&amp;", "&")

    soup = BeautifulSoup(webpage)

    skillNameArray = []
    skillLevelArray = []
    bedrijfsNaam = ""
    bedrijfsJaren = ""
    bedrijfsFunctie = ""
    opleidingsNaam = ""
    opleidingsJaren = ""
    opleidingsFunctie = ""

    patSkillName = re.compile('textboxlist(.*?)')
    skillTitleSoup = soup.findAll('div', attrs={'class':patSkillName})
    for titles in skillTitleSoup:
        patSkillName = re.compile('class="textbox(.*?)>(.*?)</div>')
        getName = re.findall(patSkillName, str(titles))
        skillName = getName.__getitem__(0)[1]
        skillNameArray.append(skillName)

    if not skillNameArray:
        badCV = True


    patSkillLevel = re.compile('percentile_icon(.*?)')
    skillLevelSoup = soup.findAll('div', attrs={'class':patSkillLevel})
    for titles in skillLevelSoup:
        patGetDigit = re.compile('percentile_icon percentile_icon_(\d*?) percentile_icon_btn test_menu')
        getDigit = re.findall(patGetDigit, str(titles))
        if getDigit:
            skillNumber = getDigit.__getitem__(0)
            skillLevelArray.append(skillNumber)

        patGetNotTested = re.compile('percentile_icon_not_tested')
        getNotTested = re.findall(patGetNotTested, str(titles))
        if getNotTested:
            skillNumber = 0
            skillLevelArray.append(skillNumber)

        patGetNotTested = re.compile('No tests available')
        getNotTested = re.findall(patGetNotTested, str(titles))
        if getNotTested:
            skillNumber = 0
            skillLevelArray.append(skillNumber)

    smallSoup = soup.findAll('div', attrs={'id':'p-overview'})

    patFindErvaring = re.compile('<h2 class="p-header" id="employmentSection" name="employmentSection">Employment</h2>(\s*?)<div class="p-cred-c">(\s*?)<div class="p-cred-title-2">(\s*?)<div class="left">(.*?)</div>(\s*?)<div class="clear"></div>(\s*?)<div class="p-cred-subtitle">(.*?)</div>(\s*?)<div class="p-cred-subtitle-3">(\n\t\t\t)(.*?)(\s*?)</div>', re.DOTALL)
    findErvaring = re.findall(patFindErvaring, str(smallSoup))
    if findErvaring:
        bedrijfsNaam = findErvaring.__getitem__(0)[3]
        bedrijfsFunctie = findErvaring.__getitem__(0)[6]
        bedrijfsJaren = findErvaring.__getitem__(0)[9]

    patFindOpleiding = re.compile('<h2 class="p-header" id="educationSection" name="educationSection">Education</h2>(\s*?)<div class="p-cred-c">(\s*?)<div class="p-cred-title-2">(\s*?)<div class="left">(.*?)</div>(.*?)<div class="clear"></div>(\s*?)<div class="p-cred-subtitle">(.*?)</div>(\s*?)<div class="p-cred-subtitle-3">(\n\t\t\t\t)(.*?)(\s*?)</div>', re.DOTALL)
    findOpleiding = re.findall(patFindOpleiding, str(smallSoup))
    print findOpleiding
    if findOpleiding:
        opleidingsNaam = findOpleiding.__getitem__(0)[3]
        opleidingsFunctie = findOpleiding.__getitem__(0)[6]
        opleidingsJaren = findOpleiding.__getitem__(0)[9]
        patFilterJaren = re.compile('(.*?)</div>(\s*?)<div class="clear">', re.DOTALL)
        filterJaren = re.findall(patFilterJaren, opleidingsFunctie)
        if filterJaren:
            opleidingsFunctie = filterJaren.__getitem__(0)[0]

    print findName
    print findNationaliteit
    print findCity
    print skillNameArray
    print skillLevelArray
    print findOmschrijving
    print bedrijfsNaam
    print bedrijfsFunctie
    print bedrijfsJaren
    print opleidingsNaam
    print opleidingsFunctie
    print opleidingsJaren
    spreektalen = "Engels"
    print spreektalen

    #godver https://www.elance.com/s/accessman/