*** Settings ***
Documentation     A test suite for mews disttributor app
...               This is a generic keyword driven test running end to end scenario
Resource          ../resource/keywords/web_app_keywords.robot

Test Teardown     Close Browser

*** Test Cases ***

User can book a room from landing page
    [Documentation]     Validates that user can successfully book a roon
    ${date} =    Get Current Date
    ${arrivalDate} =    Add Time To Date	${date}    2 days
    ${departureDate} =    Add Time To Date	${arrivalDate}    8 days
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

book another stay button should redirect to booking main page
    [Documentation]     Validates that book another stay button works
    book a stay
    click "Book another stay" button

