# -*- coding: utf-8 -*-
import re

def clearTags(curString):

    newString = curString.replace('<div class="body">', '')
    newString = newString.replace("<div>", "")
    newString = newString.replace("<strong>", "")
    newString = newString.replace("</strong>", "")
    newString = newString.replace("<h2>","")
    newString = newString.replace("</h2>","\n")
    newString = newString.replace("</H2>","\n")
    newString = newString.replace("<p>", "")
    newString = newString.replace("</p>", "\n")
    newString = newString.replace("</P>", "\n")
    newString = newString.replace("<u>", "")
    newString = newString.replace("</u>", "")
    newString = newString.replace("<b>", "")
    newString = newString.replace("</b>", "")
    newString = newString.replace("<br />", "\n")
    newString = re.sub(r"<BR(.*?)>","\n",newString)
    newString = re.sub(r"<br(.*?)>","\n",newString)
    newString = newString.replace("&amp;","&")
    newString = newString.replace("&nbsp;"," ")
    newString = newString.replace("&bull;", "- ")
    newString = newString.replace("</div>", "")
    newString = newString.replace("<ul>", "")
    newString = newString.replace("</ul>", "\n\n")
    newString = newString.replace("</UL>", "\n\n")
    newString = newString.replace("<li>", "\n- ")
    newString = newString.replace("<LI>", "\n- ")
    newString = newString.replace("</li>", "")
    newString = newString.replace("<em>", "")
    newString = newString.replace("</em>", "")
    newString = newString.replace("<ol>", "")
    newString = newString.replace("</ol>", "\n")
    newString = newString.replace("</OL>", "\n")
    newString = re.sub(r"<(.*?)>", "", newString)
    newString = newString.replace("'","")
    newString = newString.replace("`","")

    return newString

def cleanChars(curString):

    curUnicode = False
    if type(curString)==str:
        curString = curString.encode('string-escape')

    if type(curString)==unicode:
        curString = curString.encode('unicode-escape')
        curUnicode = True

    newString = re.sub("\\\\xc2\\\\(.*?...)"," ",curString) #Blankspace
    newString = re.sub("\\\\xe2\\\\x80\\\\xa8","",newString) #Blankspace

    newString = re.sub("\\\\xc3\\\\x81","Á",newString) #Á
    newString = re.sub("\\\\xc3\\\\x84","Ä",newString) #Ä
    newString = re.sub("\\\\xc3\\\\x89","É",newString) #É
    newString = re.sub("\\\\xc3\\\\x8b","Ë",newString) #Ë
    newString = re.sub("\\\\xc3\\\\x88","È",newString) #È
    newString = re.sub("\\\\xc3\\\\x8d","Í",newString) #Í
    newString = re.sub("\\\\xc3\\\\x8f","Ï",newString) #Ï
    newString = re.sub("\\\\xc3\\\\x93","Ó",newString) #Ó
    newString = re.sub("\\\\xc3\\\\x96","Ö",newString) #Ö
    newString = re.sub("\\\\xc3\\\\x9a","Ú",newString) #Ú
    newString = re.sub("\\\\xc3\\\\x9c","Ü",newString) #Ü

    newString = re.sub("\\\\xc3\\\\xa1","á",newString) #á
    newString = re.sub("\\\\xc3\\\\xa4","ä",newString) #ä
    newString = re.sub("\\\\xc3\\\\xa0","à",newString) #à
    newString = re.sub("\\\\xc3\\\\xa9","é",newString) #é
    newString = re.sub("\\\\xc3\\\\xab","ë",newString) #ë
    newString = re.sub("\\\\xc3\\\\xa8","è",newString) #è
    newString = re.sub("\\\\xc3\\\\xad","í",newString) #í
    newString = re.sub("\\\\xc3\\\\xaf","ï",newString) #ï
    newString = re.sub("\\\\xc3\\\\xb3","ó",newString) #ó
    newString = re.sub("\\\\xc3\\\\xb6","ö",newString) #ö
    newString = re.sub("\\\\xc3\\\\xba","ú",newString) #ú
    newString = re.sub("\\\\xc3\\\\xbc","ü",newString) #ü

    newString = re.sub("\\\\xe2\\\\x80\\\\x93","-",newString) #-
    newString = re.sub("\\\\xe2\\\\x80\\\\x99","",newString) #'
    newString = re.sub("\\\\xe2\\\\x80\\\\x99","",newString) #`
    newString = re.sub("\\\\xe2\\\\x80\\\\x98","",newString) #`
    newString = re.sub("\\\\xe2\\\\x80\\\\x9c","",newString) #"
    newString = re.sub("\\\\xe2\\\\x80\\\\x9d","",newString) #"
    newString = re.sub("\\\\xe2\\\\x80\\\\xa2","•",newString) #•
    newString = re.sub("\\\\xe2\\\\x82\\\\xac","€",newString) #€
    newString = re.sub("\\\\xc3\\\\x98","-",newString) #-

    newString = newString.replace('&aacute;','á')
    newString = newString.replace('&Aacute;','Á')
    newString = newString.replace('&auml;','ä')
    newString = newString.replace('&Auml;','Ä')
    newString = newString.replace('&eacute;','é')
    newString = newString.replace('&Eacute;','É')
    newString = newString.replace('&euml;','ë')
    newString = newString.replace('&Euml;','Ë')
    newString = newString.replace('&egrave;','è')
    newString = newString.replace('&Egrave;','È')
    newString = newString.replace('&iacute;','í')
    newString = newString.replace('&Iacute;','Í')
    newString = newString.replace('&iuml;','ï')
    newString = newString.replace('&Iuml;','Ï')
    newString = newString.replace('&oacute;','ó')
    newString = newString.replace('&Oacute;','Ó')
    newString = newString.replace('&ouml;','ö')
    newString = newString.replace('&Ouml;','Ö')
    newString = newString.replace('&uacute;','ú')
    newString = newString.replace('&Uacute;','Ú')
    newString = newString.replace('&uuml;','ü')
    newString = newString.replace('&Uuml;','Ü')
    newString = newString.replace('&#039;','')
    newString = newString.replace('&gt;',' ')
    newString = newString.replace('=&gt;',' ')
    newString = newString.replace('==&gt;',' ')

    if curUnicode == True:
        #aeiou
        newString = re.sub("\\\\xc1",u"Á",newString)
        newString = re.sub("\\\\xc4",u"Ä",newString)
        newString = re.sub("\\\\xe1",u"á",newString)
        newString = re.sub("\\\\xe4",u"ä",newString)
        newString = re.sub("\\\\xe0",u"à",newString)

        newString = re.sub("\\\\xc9",u"É",newString)
        newString = re.sub("\\\\xcb",u"Ë",newString)
        newString = re.sub("\\\\xe8",u"è",newString)
        newString = re.sub("\\\\xe9",u"é",newString)
        newString = re.sub("\\\\\\\\xc3\\\\\\\\xa9",u"é",newString)
        newString = re.sub("\\\\xeb",u"ë",newString)
        newString = re.sub("\\\\\\\\xc3\\\\\\\\xab",u"ë",newString)

        newString = re.sub("\\\\xcd",u"Í",newString)
        newString = re.sub("\\\\xed",u"í",newString)
        newString = re.sub("\\\\xcf",u"Ï",newString)
        newString = re.sub("\\\\xef",u"ï",newString)

        newString = re.sub("\\\\xd3",u"Ó",newString)
        newString = re.sub("\\\\xf3",u"ó",newString)
        newString = re.sub("\\\\xd6",u"Ö",newString)
        newString = re.sub("\\\\xf6",u"ö",newString)

        newString = re.sub("\\\\xda",u"Ú",newString)
        newString = re.sub("\\\\xfa",u"ú",newString)
        newString = re.sub("\\\\xdc",u"Ü",newString)
        newString = re.sub("\\\\xfc",u"ü",newString)
        newString = re.sub("\\\\u2022",u"-",newString)
        newString = re.sub("\\\\u2013100","",newString)
        newString = re.sub("\\\\u201c","",newString)
        newString = re.sub("\\\\u201d","",newString)
        newString = re.sub("\\\\u2018","",newString)

        newString = re.sub("\\\\xa0"," ",newString)

        newString = re.sub("&rsquo;","",newString) #'
        newString = re.sub("&amp;",u"&",newString)
        newString = re.sub("&lsquo;","",newString) #'
        newString = re.sub("&euro;",'',newString)
        newString = re.sub("&hellip;","",newString)
        newString = re.sub("&#39;","",newString)
        newString = re.sub("&lt;br /&gt;",u"\\n",newString)
        newString = re.sub("&quot;","",newString)
        newString = re.sub("&rdquo;","",newString) #'
        newString = re.sub("&ldquo;","",newString) #'






    return newString
