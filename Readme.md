# Mews developers

Information about development job opportunities and career at Mews Systems. If you're looking for specification of interview tasks for our open positions, check [Open positions](#open-positions). If you would like to know more about Mews, what we use, how we work etc, have a look at [About Mews](#about-mews) section.

## 💼 Open positions

Currently, we are seeking backend developers, frontend developers and Android developers of all seniority levels. As part of the interview process, we'd like to see some code. If you have something existing that you can share with us, then great! Send us a link to your public repository or send the code attached to the email. Otherwise, if you don't have anything shareable at the moment, feel free to complete one of the following tasks depending on the position you are applying for:

- [Backend task](Backend)
- [Frontend task](Frontend)
- [Android Task](Android)

## 📖 About Mews

> We have revolutionised the way that hotels operate across all departments, through our mobile hotel management platform. We enable hoteliers to free themselves from boring administration (which we help automate) and rather focus on creating real customer experiences. Now live in 40 countries in 700+ hotels, we have truly started a revolution.

That's the marketing slogan. A more down-to-earth version of it would be, that we work on a system for hotel employees and their guests. The main goal is to improve the guest experience, by giving them possibility to have full control over their stay, possibility to check-in online, manage their profile, pay online etc. In consequence, that decreases workload on the hotel employees, because the guests actually do the work. The second big goal is opening up the system via APIs to 3rd party companies and developers, so that they can build interesting applications on top of our system. Or connect it with something else, which was traditionally very difficult in hospitality industry.

In the following sections, we'll try to answer frequently asked question that we believe a candidate might have when considering application to Mews.

### 👨‍💻Technology

- **Which technologies do you use?** Generally, we try to use technology that is familiar to the development community. The benefit of using common technology is that everybody knows it well, and if not, they're able to Google the answers to their own questions. We also try to avoid building in-house solutions, but rather use third party sevices for responsibilities that are not directly related to our business (e.g. Logentries for logging, Sentry for error reporting, SendGrid for mailing). We want to focus on our product. Check our [Stackshare](https://stackshare.io/mews-systems/mews) to see a full list of what we use.
- **What is architecture of the backend?** The "executable" is a plain ASP.NET MVC application with no extra caveats. We use Entity Framework code-first approach, and we use Azure DB (a version of MSSQL) as our database. We run the application in Azure using App Services, so we don't need to manage the web servers or virtual machines. We use the original branches of .NET Framework and Entity Framework and plan to migrate to .NET Core. In general, there is data layer to access Azure DB and Azure Storage, business layer consisting of various components and a transactional layer (web, API, background jobs).
- **How is frontend strutured?** We are currently finishing migration of everything from various frameworks and libraries to a React-based stack with supporting libraries like Redux and styled-components. Frontend code development is governed by principles of functional programming and validated by automated strict linting rules and Jest-based tests. Also on frontend, we use code quality tools, mainly custom Eslint rules (99% of Airbnb rules). The long-term plan is to split all the frontend code to a separate application and have the backend just as an API service for the frontend app(s). Currently some parts of the system, especially data heavy screens like reports are rendered on server using Razor.
- **Do you create microservices?** At the moment, no. The idea of having small units that are responsible for a specific task is great, however this architecture style would not match the current needs of our system. But with increasing size of the system and teams, we might evolve there naturally.
- **What is your test coverage?** We do not know. We do not measure code coverage, primarily because we do not see it as a crucial parameter. Of course, when the project started with just two people and not much time, they were not writing any tests. However, now that we have survived the startup stage, we have begun to rectify this situation and are now currently in the stage of covering of majority of the system with end-to-end integration API and UI tests. In our opinion, they bring the biggest value for cost.
- **How often do you deploy?** We're on a path to continuous delivery, most of the deployment work is already automated, so now we deploy multiple times a week. As we improve our test base, we can be more confident to deploy much more often. Also since the frontend applications are hosted by backend, their deployment is tied together. So one of the steps forward is to make their deployment independent on backend. Some of our mobile apps, which are not offered via Appstores, are already delivered continuously.

### 🏨 Product

- **What are the applications you are building?** The biggest part is the system for employees of hotels (receptionists, housekeepers, accountatns, revenue managers, essentially everybody). For them, we also have iOS and Android application with limited functionality to some basic tasks. Then we have web and iOS applicaton for guests of the hotels. We also offer hotels free Android kiosk application that they can put into their lobby and guests can checkin there.
- **How do you handle if different hotels want different features?** We have some features that hotels can choose to enable or disable, according to their preferences. However, all of the various features are available to all hotels. Being SaaS, we have only one feature set and one live instance of the application.
- **Do you produce any open source code?** Yes, some of the things we have done are now open sourced, but as it takes a lot of time to maintain, we try to be very careful with what we decide to make open. We decided that we'll be open sourcing mainly fiscalization libraries (like EET), becuase it's annoying having to implement it when some government decides to introduce it. You can check our public repositories here https://github.com/MewsSystems.
- **Does your system have public APIs?** Yes and it's one of its strengths. We try to take it really seriously and make it as user-friendly as possible for other developers to integrate with us. We also have a dedicated team to help the integration partners and give them advices how to best connect to us. So now, we have over 150 companies consuming our APIs. You can find out more here: https://mews-systems.gitbook.io/connector-api/

### ⛹️Teamwork

### 🏢 Workplace
