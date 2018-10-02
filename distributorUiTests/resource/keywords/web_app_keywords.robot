*** Settings ***

Documentation   Importing web and robotframework built-in keywords.
Library  Selenium2Library   timeout=10  #run_on_failure=CapturePageScreenShot
Library   ../../resource/common_lib/Common.py
Library   DateTime
Library   String
Resource          ../../resource/settings/ObjectIds.robot

*** Variables ***
${SERVER}         https://demo.mews.li/distributor
${BROWSER}        chrome
${DELAY}          0
${DISTRIBUTOR_ID}    257ec78c-7af6-44e0-819f-0d8465bb35bf
${RETRY}    4
${MIN RETRY INTERVAL}    5          #secconds


*** Keywords ***
#########################################################
browser is opened to landing page
    [Documentation]
    [Tags]
    Open Browser    ${SERVER}/${DISTRIBUTOR_ID}    ${BROWSER}
    Maximize Browser Window
    Set Selenium Speed    ${DELAY}
    Landing Page Should Be Open

select frame on page
    [Documentation]    selects the iframe on home page to be able to interact with page elements
    [Tags]
    Wait Until Page Contains Element    ${page iframe}     20
    Select Frame    ${page iframe}

Landing Page Should Be Open
    [Documentation]
    [Tags]
    select frame on page
    Title Should Be    ${page title}
    Wait Until Page Contains Element    xpath=//*[@data-test-id="dates-view"]
    Wait Until Page Contains Element    ${calendar departure text heading element}
    Page Should Contain Element    ${calendar arrival text heading element}
    Unselect Frame

click NEXT button from the calendar on home page
    [Documentation]    clicks on the NEXT button from the calendar on the home page frame
    [Tags]
    select frame on page
    Wait Until Page Contains Element    ${calendar next button}   20
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element   ${calendar next button}
    Wait Until Page Does Not Contain Element    ${calendar next button}    20
    Unselect Frame

set calendar month ${displayedCalendarMonthYearNum}
    Set Test Variable    ${displayedCalendarMonthYearNum}    ${displayedCalendarMonthYearNum}

user selects desired arrival date from day: ${day} - ${month} - ${year} from the home page
    [Documentation]     arrival date
    select frame on page
    ${month}=   Capitalize First Letter Of A Word  ${month}
    ${day} =    Fetch From Right   ${day}    0
    ${expectedMonthYear}=   Catenate    ${month}  ${year}
    ${expectedMonthNum}=    get number value of month  ${month}
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    ${calendar launch button}
    ${displayedMonthYear}=   Get Datepicker MonthYear ${1}
    Wait Until Page Contains Element    xpath=(//*[contains(@class, "style__DayButtonElement") and contains(., "${day}")])[1]    20
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    xpath=(//*[contains(@class, "style__DayButtonElement") and contains(., "${day}")])[1]
    Unselect Frame

user selects desired departure date from day: ${day} - ${month} - ${year} from the home page
    [Documentation]     departure date
    select frame on page
    ${month}=   Capitalize First Letter Of A Word  ${month}
    ${expectedMonthYear}=   Catenate    ${month}  ${year}
    ${day} =    Fetch From Right   ${day}    0
    ${expectedMonthYearNum}=    get number value of month  ${expectedMonthYear}
    ${displayedMonthYear1}=   Get Datepicker MonthYear ${1}
    ${displayedMonthYear2}=   Get Datepicker MonthYear ${2}
    ${displayedMonthYearNum2}=    get number value of month  ${displayedMonthYear2}
    ${displayedMonthYearNum1}=    get number value of month  ${displayedMonthYear1}

#    ${displayedMonthNum}=    get number value of month  ${displayedMonthYear}
#    :FOR    ${Index}    IN RANGE    1   12
#    \   Run Keyword If  '${displayedMonthyear}' == '${expectedMonthYear}'   Exit For Loop
#    \   Run Keyword If  '${displayedMonthNum}' > '${expectedMonthNum}'   Click Element    xpath=//button[contains(@class, "style__PrevMonthIconButton")]
#    \   Run Keyword If  '${displayedMonthNum}' < '${expectedMonthNum}'   Click Element    xpath=//button[contains(@class, "style__NextMonthIconButton")]
#    \   sleep    2
#    \   ${displayedMonthyear}=   Get Datepicker MonthYear ${2}

    Run Keyword If    '${expectedMonthYearNum}' == '${displayedMonthYearNum1}'    Wait Until Page Contains Element   xpath=(//*[contains(@class, "style__DayButtonElement") and contains(., "${day}")])[1]    20
    Run Keyword If    '${expectedMonthYearNum}' == '${displayedMonthYearNum1}'    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    xpath=(//*[contains(@class, "style__DayButtonElement") and contains(., "${day}")])[1]

    Run Keyword If    '${expectedMonthYearNum}' == '${displayedMonthYearNum2}'    Wait Until Page Contains Element   xpath=(//*[contains(@class, "style__DayButtonElement") and contains(., "${day}")])[2]    20
    Run Keyword If    '${expectedMonthYearNum}' == '${displayedMonthYearNum2}'    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    xpath=(//*[contains(@class, "style__DayButtonElement") and contains(., "${day}")])[2]
    Unselect Frame

select category "${category}"
    [Documentation]     selects room category provided
    select frame on page
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    xpath=(.//*[normalize-space(text()) and normalize-space(.)='${category}'])
    Unselect Frame

book room with: ${rate type} rates
    select frame on page
    Wait Until Page Contains Element    xpath=(//*[contains(@class, "style__ButtonElement") and contains(., "Book now")])[1]
    Run Keyword If  '${rate type}' == 'flexible'    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    ${book now button flexible rate}
    Run Keyword If  '${rate type}' == 'Neg'    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    ${book now button neg rate}
    Unselect Frame

select first item in rooms categories
    [Documentation]     selects first item in list of categories provided
    select frame on page
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    xpath=(//*[contains(@type, "button") and contains(., "Show rates")])[1]
    Unselect Frame

click "${button name}" button
    select frame on page
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element    xpath=(//*[contains(@type, "button") and contains(., "${button name}")])
    Unselect Frame


page heading should be "${text}"
    select frame on page
    Wait Until Page Contains    ${text}    20
    Unselect Frame

Get Datepicker MonthYear ${leg}
    [Documentation]     Return current month + year from datepicker
    [Return]    ${monthyear}
    Wait Until Page Contains Element    css=div[class^="style__MonthGridElement"]> div:nth-child(${leg})> div[class^="style__MonthNameElement"]    30
    ${monthYear}=    Get Text    css=div[class^="style__MonthGridElement"]> div:nth-child(${leg})> div[class^="style__MonthNameElement"]

Unique Title
    [Documentation]
    [Return]  ${time}
    ${secs}=    Get Time    epoch
    ${time}=    Convert To String     ${secs}
#    ${title}=    Catenate  SEPRATOR=-    auto    ${time}

fill users details
    [Documentation]     fills details form and enters nationality as CZ user
    select frame on page
    ${surfix}=  Unique Title
    Wait Until Page Contains Element    xpath=//*[@name="email" and contains(@class, "style__InputElement")]
    Input Text    xpath=//*[@name="email" and contains(@class, "style__InputElement")]   auto-email-${surfix}@mews.com
    Input Text    xpath=//*[@name="firstName" and contains(@class, "style__InputElement")]   auto-first-name-${surfix}
    Input Text    xpath=//*[@name="lastName" and contains(@class, "style__InputElement")]   auto-last-name-${surfix}
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element        xpath=(.//*[normalize-space(text()) and normalize-space(.)='Nationality'])[1]/following::span[1]
    Input Text    xpath=(.//*[normalize-space(text()) and normalize-space(.)='Nationality'])[1]/following::input[1]   Cz
    Wait Until Keyword Succeeds    ${RETRY} times    ${MIN RETRY INTERVAL} s    Click Element        //*[@value="CZ" and contains(@class, "style__ListItemContainerElement")]
    Input Text    xpath=//*[@name="notes" and contains(@class, "style__TextareaElement")]   auto-NA
    Click Element    xpath=//*[@data-test-id="terms-and-conditions-checkbox" and contains(@class, "style__InputElement")]
    Unselect Frame

booking confirmation page should contain arrival date: ${expectedArrivalDate} and departure date: ${expectedDepartureDate}
    select frame on page
    ${arrivals}=    Get Text   ${arrival date on confirmation date}
    ${departures}=    Get Text   ${departure date on confirmation date}
    Should Be Equal	'${arrivals}'	'${expectedArrivalDate}'
    Should Be Equal	'${departures}'	'${expectedDepartureDate}'
    Unselect Frame


book a stay
    [Documentation]
    ${date} =    Get Current Date
    ${arrivalDate} =    Add Time To Date	${date}    2 days
    ${departureDate} =    Add Time To Date	${arrivalDate}    2 days
    ${arrivalDate} =	Convert Date	${arrivalDate}	result_format=%d/%m/%Y
    ${departureDate} =	Convert Date	${departureDate}	result_format=%d/%m/%Y
    ${arrivalDay} =    Fetch From Left   ${arrivalDate}    /

    ${arrivalYear} =    Fetch From Right   ${arrivalDate}    /
    ${arrivalMonth} =    Remove String    ${arrivalDate}    ${arrivalDay}/    /${arrivalYear}
    ${departureDay} =    Fetch From Left   ${departureDate}    /


    ${departureYear} =    Fetch From Right   ${departureDate}    /
    ${departureMonth} =    Remove String    ${departureDate}    ${departureDay}/    /${departureYear}
    ${departureMonth}=    get_text_value_of_month   ${departureMonth}
    ${arrivalMonth}=    get_text_value_of_month   ${arrivalMonth}

    log to console    ${arrivalDay}
    browser is opened to landing page
    user selects desired arrival date from day: ${arrivalDay} - ${arrivalMonth} - ${arrivalYear} from the home page
    user selects desired departure date from day: ${departureDay} - ${departureMonth} - ${departureYear} from the home page
    click NEXT button from the calendar on home page
    page heading should be "${category page heading}"
    select first item in rooms categories
    page heading should be "${Rate page heading}"
    book room with: flexible rates
    page heading should be "${Summary page heading}"
    click "Proceed to checkout" button
    page heading should be "${Guest details page heading}"
    fill users details
    click "Finish booking" button
    page heading should be "${Confirmation page heading}"
    Page Should Contain    ${Confirmation page message}
    booking confirmation page should contain arrival date: ${arrivalDate} and departure date: ${departureDate}

