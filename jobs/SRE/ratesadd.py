import csv

with open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\ratesadd.txt", "rt") as fo:
    li = fo.readlines()
    for rec in li:
        output = (rec.split('"')[3] + "," +rec.split('":')[4]) 
        output = (output.split('}')[0])
        print (output)