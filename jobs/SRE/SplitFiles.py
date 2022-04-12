import csv

#Split out Enpoints into seperate files
open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\reservationsadd.txt",'w').writelines(line for line in open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\Logs.txt","rt") if "reservations/add" in line)
open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\ratesadd.txt",'w').writelines(line for line in open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\Logs.txt","rt") if "rates/add" in line)
open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\reservationsget.txt",'w').writelines(line for line in open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\Logs.txt","rt") if "reservations/get" in line)
open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\reservationsupdate.txt",'w').writelines(line for line in open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\Logs.txt","rt") if "reservations/update" in line)
open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\ratesupdate.txt",'w').writelines(line for line in open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\Logs.txt","rt") if "rates/update" in line)
open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\reservationscancel.txt",'w').writelines(line for line in open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\Logs.txt","rt") if "reservations/cancel" in line)
open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\ratesdelete.txt",'w').writelines(line for line in open(r"C:\Users\tomlm\MewsSRETest\developers\jobs\SRE\Logs.txt","rt") if "rates/delete" in line)

