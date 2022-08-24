# Mews SRE task

You are given a file `logs.txt` with JSON log items containing information about requests on production for some time interval. The item contains various data, but most importantly endpoint name and the request duration. Several deploys happened in the given interval with bugs that made some endpoints much slower (regressed).

Your task is to find which requests regressed and also the approximate time of it with 5-minute tolerance.
Write a program in any language you would like (or even pseudocode) that reads this file and prints out the result.

Note that it is up to you to define what regression means.

**Input**: `logs.txt.` file.

**Output**: Name of endpoints that regressed and the time when it approximately happened.

Note that the quality of the code is not the most important aspect, since this is not a task for a backend engineer.
Since the task is pretty vague, it is also ok to make some reasonable assumptions about the form of regression. Just make sure to state them with your solution.

## My approach

Since I am mostly familiar with Python, I will use it for writing my code.

Given the explorative nature of this task, I will actually build the code in an interactive notebook, which allows me to also include comments, plots and other visual information with ease.

All my tought process, findings and conclusions can be found in the notebook output.

I have included the notebook output (code, findings, analysis) in the `out/` folder, both in HTML and PDF format. This way you don't have to install anything to read and browse it.

I have also included a minimal script to do this task in `task.py`, but I would invite you to look instead at the analysis I have in the notebook output. The script just solves the task without any explaination.

### Requirements

To run my code, the following will be necessary on your system:

* Python, any recent (3.7+) version should work, but I have developed and tested it with Python 3.10
    * Verify this with `python --version`

I'm writing this code from a Linux machine, but it should work just fine on Windows.

### Running my code

* Create a Python virtual environment to install the various dependencies
    * `python -m venv venv/`
* Activate the virtual environment
    * `source venv/bin/activate`
* Install the dependencies for the project
    * `pip install -r requirements.txt`
    * If this fails because the dependencies are not supported by your Python version, you can see if the code works by `pip install numpy matplotlib pandas jupyterlab`
* You can now start the notebook by running `jupyer lab`, a browser window should open (or you should find instructions on how to open it in the console)
