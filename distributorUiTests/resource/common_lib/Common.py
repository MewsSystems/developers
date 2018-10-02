import requests
import base64
import datetime
import calendar

def capitalize_first_letter_of_a_word(inputString):
    return inputString.title()

def get_number_value_of_month(strMonth):
    strMonth = ''.join([i for i in strMonth if not i.isdigit()]).strip()
    month_to_num = {name: num for num, name in enumerate(calendar.month_name) if num}
    return month_to_num[strMonth]

def get_text_value_of_month(intMonth):
    return calendar.month_name[int(intMonth)]

def remove_trailing_zero(my_string):
    return my_string.strip("0")

