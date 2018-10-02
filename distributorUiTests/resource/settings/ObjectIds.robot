*** Variables ***
${page title}    New booking
${page iframe}    xpath=/html/body/iframe
${calendar departure text heading element}    xpath=//*[@id="portal-container" and contains(., "Departure")]
${calendar arrival text heading element}    xpath=//*[@id="portal-container" and contains(., "Arrival")]
${calendar next button}    xpath=//*[@data-test-id="dates-next-button"]
${calendar launch button}    xpath=(.//*[normalize-space(text()) and normalize-space(.)='Arrival'])
${book now button flexible rate}    xpath=(//*[contains(@class, "style__ButtonElement") and contains(., "Book now")])[2]
${book now button neg rate}    xpath=(//*[contains(@class, "style__ButtonElement") and contains(., "Book now")])[1]
${arrival date on confirmation date}    css=div > div:nth-child(2) > span > span:nth-child(1) > span:nth-child(2)
${departure date on confirmation date}    css=div > div:nth-child(2) > span > span:nth-child(2) > span:nth-child(2)

${category page heading}    Select category
${Rate page heading}    Select rate
${Summary page heading}    Summary
${Guest details page heading}    Guest details
${Confirmation page heading}    Confirmation
${Confirmation page message}    Thank you for your reservation at StudentHouse! You will receive a confirmation email containing additional details.





