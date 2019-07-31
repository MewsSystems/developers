# Mews developers

Information about development job opportunities and career at [Mews Systems](https://www.mewssystems.com). If you're looking for specification of interview tasks for our open positions, check [Open positions](#open-positions). If you would like to know more about Mews, what we use, how we work etc, have a look at [About Mews](#about-mews) section. We also have a [Blog](https://developers.mewssystems.com) and [Twitter](https://twitter.com/MewsDevs) with latest information.

**You can always reach us at `jobs@mewssystems.com`.**

<a name='open-positions'/>

## 💼 Open positions

Currently, we are seeking backend developers, frontend developers, data analysts and Android developers of all seniority levels. For all current openings, even outside development, check our open positions on [Workable](https://mews-systems.workable.com/). If you don't match exactly any of the candidate profiles, but you feel you could contribute in some other way, let us know as well and we can figure it out.

As part of the interview process, we'd like to see some code. If you have something existing that you can share with us, then great! Send us a link to your public repository or send the code attached to the email. Otherwise, if you don't have anything shareable at the moment, feel free to complete one of the following tasks depending on the position you are applying for. If you'd like to first talk to us, that's an option as well, the task is not a mandatory thing.

- [Task](Backend) for backend candidates:
    - [Senior backend developer](https://mews-systems.workable.com/j/AA7C6F1D43?viewed=true)
    - [Backend developer](https://mews-systems.workable.com/j/A351B0CD2C?viewed=true)
- [Task](Frontend) for frontend candidates:
    - [Senior frontend developer](https://mews-systems.workable.com/j/FDC35CE6E9?viewed=true)
    - [Frontend developer](https://mews-systems.workable.com/j/02C70D4EE1?viewed=true)
- [Task](Android) for Android candidates.

<a name='about-mews'/>

## 📚 About Mews 

> We have revolutionised the way that hotels operate across all departments, through our mobile hotel management platform. We enable hoteliers to free themselves from boring administration (which we help automate) and rather focus on creating real customer experiences. Now live in 45+ countries in 1000+ hotels, we have truly started a revolution.

That's the marketing slogan. A more down-to-earth version of it would be, that we work on a system for hotel employees and their guests. The main goal is to improve the guest experience, by giving them possibility to have full control over their stay, possibility to check-in online, manage their profile, pay online etc. In consequence, that decreases workload on the hotel employees, because the guests actually do the work. The second big goal is opening up the system via APIs to 3rd party companies and developers, so that they can build interesting applications on top of our system. Or connect it with something else, which was traditionally very difficult in hospitality industry. If you'd like to know more, check this sample [sales-pitch](https://www.facebook.com/MewsSystems/videos/351045845603219/) which explains what we offer to our clients and which problems we solve.

In the following sections, we'll try to answer frequently asked question that we believe a candidate might have when considering application to Mews:

- [Product](#product) - what we build.
- [Technology](#technology) - how we build it.
- [Teamwork](#teamwork) - how do we cooperate.
- [Job](#job) - offices, working hours, salaries.
- [Relocation](#relocation) - information for people outside CZ.

<a name='product'/>

### 🏨 Product 

- **What are the applications you are building?** The biggest part is the system for employees of hotels (receptionists, housekeepers, accountants, revenue managers, essentially everybody). For them, we also have iOS and Android application with limited functionality to some basic tasks. Then we have web and iOS applicaton for guests of the hotels. We also offer hotels free Android kiosk application that they can put into their lobby and guests can checkin there. 
- **How do you handle if different hotels want different features?** We have some features that hotels can choose to enable or disable, according to their preferences. However, all of the various features are available to all hotels. Being SaaS, we have only one feature set and one live instance of the application.
- **Do you produce any open source code?** Yes, some of the things we have done are now open sourced, but as it takes a lot of time to maintain, we try to be very careful with what we decide to make open. We decided that we'll be open sourcing mainly fiscalization libraries (like EET), becuase it's annoying having to implement it when some government decides to introduce it. You can check our public repositories here https://github.com/MewsSystems.
- **Does your system have public APIs?** Yes and it's one of its strengths. We try to take it really seriously and make it as user-friendly as possible for other developers to integrate with us. We also have a dedicated team to help the integration partners and give them advices how to best connect to us. So now, we have over 150 companies consuming our APIs. You can find out more here: https://mews-systems.gitbook.io/connector-api/

<a name='technology'/>

### 👨‍💻 Technology 

- **Which technologies do you use?** Generally, we try to use technology that is familiar to the development community. The benefit of using common technology is that everybody knows it well, and if not, they're able to Google the answers to their own questions. We also try to avoid building in-house solutions, but rather use third party sevices for responsibilities that are not directly related to our business (e.g. Logentries for logging, Sentry for error reporting, SendGrid for mailing). We want to focus on our product. Check our [Stackshare](https://stackshare.io/mews-systems/mews) to see a full list of what we use.
- **What is architecture of the backend?** The "executable" is a plain ASP.NET MVC application with no extra caveats. We use Entity Framework code-first approach, and we use Azure DB (a version of MSSQL) as our database. We run the application in Azure using App Services, so we don't need to manage the web servers or virtual machines. We use the original branches of .NET Framework and Entity Framework and plan to migrate to .NET Core. In general, there is data layer to access Azure DB and Azure Storage, business layer consisting of various components and a transactional layer (web, API, background jobs).
- **How is frontend strutured?** We are currently finishing migration of everything from various frameworks and libraries to a React-based stack with supporting libraries like Redux and styled-components. Frontend code development is governed by principles of functional programming and validated by automated strict linting rules and Jest-based tests. Also on frontend, we use code quality tools, mainly custom Eslint rules (99% of Airbnb rules). The long-term plan is to split all the frontend code to a separate application and have the backend just as an API service for the frontend app(s). Currently some parts of the system, especially data heavy screens like reports are rendered on server using Razor.
- **How does technology stack for mobile apps look like?** iOS and Android versions of Commander are native apps written in Swift and Java correspondingly (with latest one being migrated to Kotlin). Operator kiosk application is written from scratch in Kotlin. For Android development we use a standard set of libraries: Dagger, Retrofit, RxKotlin (and Detekt for static code analysis). We have a fully set up CI/CD process for Operator app and are moving into this direction for other apps as well. We're also experimenting with Flutter in order to understand whether it fits well into our tasks.
- **Do you create microservices?** At the moment, no. The idea of having small units that are responsible for a specific task is great, however this architecture style would not match the current needs of our system. But with increasing size of the system and teams, we might evolve there naturally.
- **What is your test coverage?** We do not know. We do not measure code coverage, primarily because we do not see it as a crucial parameter. Of course, when the project started with just two people and not much time, they were not writing any tests. However, now that we have survived the startup stage, we have begun to rectify this situation and are now currently in the stage of covering of majority of the system with end-to-end integration API and UI tests. In our opinion, they bring the biggest value for cost.
- **How often do you deploy?** We're on a path to continuous delivery, most of the deployment work is already automated, so now we deploy multiple times a week. As we improve our test base, we can be more confident to deploy much more often. Also since the frontend applications are hosted by backend, their deployment is tied together. So one of the steps forward is to make their deployment independent on backend. Some of our mobile apps, which are not offered via Appstores, are already delivered continuously.

<a name='teamwork'/>

### ⛹️ Teamwork 

- **How often do you schedule refactoring?** We do not schedule refactoring. We consider it to be an integral part of development work. When implementing a new feature, it is important to verify that all of the team code and architecture quality standards are applied. The fixes and features should all be done properly from the beginning.
- **How do you handle changes in the architecture of the system?** We try to introduce changes gradually while extending the capabilities of our system. For bigger changes, both on backend and frontend, we currently have "platform" teams whose only responsibility is to improve the system architecture, develop libraries and tools for other teams that are more focused on "product".
- **How is your workload structured and prioritized?** From top-level perspective, if we would sum up all the issues in some time interval, we try to keep ratio 1:1:1 between following types of issues. This applies to all teams, both platform and product teams.
    - Core feature - something that we need to build because it's integral part of the business.
    - Differentiator - something that makes us stand our from competition.
    - Consolidation - refactoring, paying-off technical debt, improvements.
- **Which versioning system do you use?** We host our code on GitHub, therefore we use git.
- **What is your branching workflow?** [Gitflow](https://nvie.com/posts/a-successful-git-branching-model/). On projects that are  continuously delivered, we have only master branch and feature branches, but that's just Gitflow without some branches.
- **Do you do code reviews?** Yes, for everything. Code review is a crucial part of how we work. We consider this a great practice, which helps the whole team understand what is happening in the system. As a developer, you not only gather feedback on your code (which helps you to improve), but you're also able to spread your knowledge and experience to others. It's also great platform for newcomers and juniors to ask questions and learn. For code-reviews we use Github pull requests. Currently, each pull request is reviewed by at least two people, one being senior.
- **Do you work with continuous integration?** Once you open a pull request, our CI server (TeamCity) picks it up automatically and runs checks against the changes. Unless all checks have passed, you will not be able to merge your pull request.
- **Do you have any code style rules?** We believe in naming things the right way rather than adding comments. We have our own set of code style rules (99% of .NET FW rules on backend AirBnb on frontend) that are run during any build. If your code violates any of the rules, no one will be able to build the project. The goal is to have a uniform code style throughout the whole platform. We also create our own analyzers for common mistakes that appear during code-reviews.
- **Do you look for any specialists?** Not really. We hire "generic" developers and try to learn about their individual skills over time. Once we know what they enjoy working on, we try to embrace that and assign projects that each person is interested in. We have enough of a workload for the generic developers, which extends to all layers of our application including new features and/or fixing bugs. At the moment, we have 8 teams with different responsibilities, so there are options to choose from.
- **How many meetings would I have in a day?** That's up to team to decide, in most teams there is a short daily standup meeting. Each team also has an open weekly meeting (around 30 minutes), that anyone in the company can join to see what the team is currently up-to, what is planned for short-term future.
- **What is the company language?** In Prague office, where development is located, around 50% of people are czech and the rest is mix of many other nationalities. Therefore most of the meetings and communication is held in English. We also have native English speakers, some of them former English teachers in Prague, so we offer lessons for people who need to sharpen their language skills.

<a name='job'/>

### 🏢 Job

- **What does the office look like?** The office is located at [I.P. Pavlova square, 1789/5](https://www.google.com/maps/place/n%C3%A1m%C4%9Bst%C3%AD+I.+P.+Pavlova+1789%2F5,+120+00+Nov%C3%A9+M%C4%9Bsto/@50.0752762,14.4298992,3a,75y,184.78h,101.94t/data=!3m6!1e1!3m4!1s73pkxL_pvma3Oge6NEn3QQ!2e0!7i13312!8i6656!4m5!3m4!1s0x470b948c0ea2c643:0x3f011e15da2b48b!8m2!3d50.0749916!4d14.4298445), which is great in terms of transport options (metro line C, A, many trams).
- **What benefits do you offer?** We have Rohlík.cz coming every day to deliver a variety of fruit, crisps, cookies, coke and other yummy things - we prefer to keep our fridge rather full. However, these snacks are not a substitute for regular meals. In addition to snacks, we have batch brew beans from ČerstváKáva.cz delivered every month, which means there is a huge supply of fresh, quality coffee in the office at all times. We have just recently abandoned the stage of being a startup. The environment in our office is much more similar to a startup than to that of a corporate one, but although it is changing, we do not offer most of the corporate benefits (company car, mobile plans, meal tickets, etc.). We provide these things to make the office environment more pleasant.
- **Do you pay out special bonuses?** No, we do not. We prefer to pay good salaries transparently and on a regular basis so that you always know what you can count on.
- **How many years would it take to reach a senior position?** We have a bit of a different understanding of what "senior developer" means. To us, a senior developer takes full responsibility for some part or component of the system and does not need anyone to help them with their tasks or to look over their shoulder. For this reason, we cannot guarantee that you will become a senior developer. It's up to you to determine how quickly you can become that skilled. Furthermore, if you consider yourself to be a senior developer already, it may happen that we will not agree. In our job posts for senior positions, you can find a list of topics. A senior developer should ideally have deeper knowledge with at least some of them.
- **Is it possible that a new junior developer would earn a higher salary than me as a senior developer?** No. We try to avoid frustrations about money and keep an eye on the salary of each person individually; we believe it should correspond to their skill level. Therefore, if we would desperately need to hire a junior developer with a salary higher than yours, we would increase all the salaries in the company so that the levels are kept intact. We want to pay salaries that are competitive to CZ market as we don't want you to leave because of a better salary offer. We don't have open salaries, but try to structure them as if they were.
- **Can I have a home office?** Yes, but there are some rules to follow:
    - Home office is a thing of trust. We need to be able to trust that you are producing the same amount and quality of work as when working from the office. If you have a home office but do not respond for 2 hours on Slack, that's suspicious. Home office is not a day off.
    - When taking home office, we expect that you do not need anyone to help you accomplish your tasks that day and that you will not need to interrupt people via any communication channel.
    - When you start working with us, you should probably spend the first several weeks in the office, as otherwise, you would surely be in conflict with rule no. 2.
- **What are the working hours?** That really depends on you. Every person has a different rhythm to their life. As long as you accomplish what you have promised to accomplish, it's up to you. Most people are in the office between 11:00 and 17:00. Although there is a lot of flexibility, your working hours should intersect with this time period so that you have opportunities to meet with the team and consult them regarding your work.

<a name='relocation'/>

### ✈️ Relocation 

- **Do you help with relocation?** Yes, we do. We have an agency that would contact you to help you resolve any paperwork you might need for both our government and your own. We also provide accommodation for the first few months until you are able to find your own apartment. We have "Mews flat" in Prague and London which we manage using our own system.
- **What is the cost of living in Prague?** You can find it and compare with other cities at https://www.numbeo.com/cost-of-living/in/Prague.
- **How long does it take to relocate?** Unfortunately, it takes a lot of time; usually around 6 months. First, we must submit the the job posting announcing your position to the Czech Labour office, where it must be pending publicly for 30 days for potential employees. After that, the paperwork is processed, and both governments have 30-60 days to finish the procedure. Regrettably, these offices like to take their time and therefore, it is difficult to give an exact estimate about how long it takes from start to finish.
