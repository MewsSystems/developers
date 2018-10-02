=====================================================
Mews distributor's Web testing with Robot Framework and Selenium2Library
=====================================================

`Robot Framework`_ is a generic open source test automation framework and
Selenium2Library_ is one of the many test libraries that can be used with
it. In addition to showing how they can be used together for web testing,
this demo introduces the basic Robot Framework test data syntax, how tests
are executed, and how logs and reports look like.

.. contents:: **Contents:**
   :depth: 1
   :local:

Test Suites
==========

Test case files as well as a resource file used by them are located in
- testusites are located in dir ``tests``
- resources used at ``test case`` of test suite levels are located in ``resource`` dir and other dependent files are located and
  arranged in directories with logical names e.g ``keywords``, ``settings``

See `Robot Framework User Guide`_ for more details about the test data syntax.

Generated results
=================

After `running tests`_ you will get report and log in HTML format and junit style report for integration with third party CI reports.
Running demo
============

Preconditions
-------------

A precondition for running the tests is having `Robot Framework`_ and
Selenium2Library_ installed, and they in turn require
Python_. Robot Framework `installation instructions`__ cover both
Robot and Python installations, and Selenium2Library has its own
`installation instructions`__.

In practice it is easiest to install Robot Framework and
Selenium2Library along with its dependencies using `pip`_ package
manager. Once you have pip installed, all you need to do is running
these commands::

    pip install robotframework
    pip install robotframework-selenium2library

__ https://github.com/robotframework/robotframework/blob/master/INSTALL.rst
__ https://github.com/robotframework/Selenium2Library/blob/master/INSTALL.rst

For this demo a requirement.txt file is provided. it is enough to just run

    pip install requirements.txt

Running tests
-------------
assumption is that chrome driver is already installed on the execution machine

The `test cases`_ are located in the ``tests`` directory. They can be
executed using the ``robot`` command::

    robot tests

.. note:: If you are using Robot Framework 2.9 or earlier, you need to
          use the ``pybot`` command instead.

You can also run an individual test case file and use various command line
options supported by Robot Framework::

    robot tests/distributor.robot


Run ``robot --help`` for more information about the command line usage and see
`Robot Framework User Guide`_ for more details about test execution in general.

Using different browsers
------------------------

The browser that is used is controlled by ``${BROWSER}`` variable defined in
`resource.robot`_ resource file. Firefox browser is used by default, but that
can be easily overridden from the command line::

    robot --variable BROWSER:Chrome tests
    robot --variable BROWSER:IE tests

Consult Selenium2Library_ documentation about supported browsers. Notice also
that other browsers than Firefox require separate browser drivers to be
installed before they can be used with Selenium and Selenium2Library.

.. _Robot Framework: http://robotframework.org
.. _Selenium2Library: https://github.com/robotframework/Selenium2Library
.. _Python: http://python.org
.. _pip: http://pip-installer.org
.. _download page: https://bitbucket.org/robotframework/webdemo/downloads
.. _source code: https://bitbucket.org/robotframework/webdemo/src
.. _valid_login.robot: https://bitbucket.org/robotframework/webdemo/src/master/login_tests/valid_login.robot
.. _invalid_login.robot: https://bitbucket.org/robotframework/webdemo/src/master/login_tests/invalid_login.robot
.. _gherkin_login.robot: https://bitbucket.org/robotframework/webdemo/src/master/login_tests/gherkin_login.robot
.. _resource.robot: https://bitbucket.org/robotframework/webdemo/src/master/login_tests/resource.robot
.. _report.html: http://robotframework.bitbucket.org/WebDemo/report.html
.. _log.html: http://robotframework.bitbucket.org/WebDemo/log.html
.. _Robot Framework User Guide: http://robotframework.org/robotframework/#user-guide
