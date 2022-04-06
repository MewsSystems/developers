
#!/usr/bin/env python3

import json
import pandas as pd

# list of slow requests
slow_endpoints = []

# read the logs.txt file
def read_logs() :
    try:
        with open('logs.txt') as file:
            for line in file:
                # convert string to dictionary
                res_dic = json.loads(line.rstrip())
                if res_dic['Duration'] >= 200:
                    # create datetime object from the string
                    ts = pd.Timestamp(res_dic['TimeStamp'])
                    # update the TimeStamp with a rounded time
                    res_dic['TimeStamp'] = ts.round(freq='5T').strftime('%Y-%m-%d %X')
                    # Remove this key to easily remove duplicates later on
                    del res_dic['Duration']
                    slow_endpoints.append(res_dic)
        return True
    except:
        print("Error when reading logs file")
        return False

# save results to a file instead of printing to standard output
def save_to_file () :
    # remove duplicate requests to show only one request per 5min timeframe
    d_unique = pd.DataFrame(slow_endpoints).drop_duplicates().to_dict('records')
    with open('slow_endpoints.txt', 'w') as file:
        for item in d_unique:
            line = f"{item['TimeStamp']} \t {item['Endpoint']}\n"
            file.write(f"{line}")

if __name__ == "__main__":
    
    if read_logs() :
        save_to_file ()
