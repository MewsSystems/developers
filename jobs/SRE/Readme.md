# Mews SRE task

You are given a file `logs.txt` with JSON log items containing information about requests on production for some time interval. The item contains various data, but most importantly endpoint name and the request duration. Several deploys happened in the given interval with bugs that made some endpoints much slower (regressed).

Your task is to choose any language to write a program, that reads this file and prints out which requests regressed and also the approximate time of it with 5-minute tolerance. It is up to you to define what regression means.

**Input**: `logs.txt.` file.

**Output**: Name of endpoints that regressed and the time when it approximately happened.

Note that the quality of the code is not the most important aspect, since this is not a task for a backend engineer.
Since the task is pretty vague, it is also ok to make some reasonable assumptions about the form of regression. Just make sure to state them with your solution.
