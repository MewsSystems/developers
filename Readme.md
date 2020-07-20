# Mews developers

Information about development job opportunities and career at [Mews](https://mews.com). If you're looking for specification of interview tasks for our open positions, check [Open positions](#open-positions). If you would like to know more about Mews, what we use, how we work etc, have a look at [About Mews](#about-mews) section. We also have a [Blog](https://developers.mews.com), [Twitter](https://twitter.com/MewsDevs) and [Facebook](https://www.facebook.com/MewsDevs/) with latest news about events, tech stack and life of devs at Mews.

**You can always reach us at [`tech.cm@mews.com`](mailto:tech.cm@mews.com).**

<a name='open-positions'/>

## 💼 Open positions

Currently, we are seeking highly experienced people for the following positions:
- [Senior Backend Developers - .NET C#](https://www.mews.com/careers/jobs/senior-backend-developer-.NETC-007)
- [Senior Azure Architect](https://www.mews.com/careers/jobs/senior-azure-architect-008)
- [Senior Frontend Developers for a Design System](https://www.mews.com/careers/jobs/senior-frontend-developer-for-a-design-system009)
- [Data Engineer](https://www.mews.com/careers/jobs/data-engineer010)

For all current openings, even outside development, check our [open positions](https://www.mews.com/careers/jobs). If you don't match exactly any of the candidate profiles, but you feel you could contribute in some other way, let us know as well and we can figure it out.

<a name='about-mews'/>

## 📚 About Mews 

> We have revolutionised the way that hotels operate across all departments, through our mobile hotel management platform. We enable hoteliers to free themselves from boring administration (which we help automate) and rather focus on creating real customer experiences. Now live in 60+ countries in 1600+ hotels, we have truly started a revolution.

That's the marketing slogan. A more down-to-earth version of it would be, that we work on a system for hotel employees and their guests. The main goal is to improve the guest experience, by giving them possibility to have full control over their stay, possibility to check-in online, manage their profile, pay online etc. In consequence, that decreases workload on the hotel employees, because the guests actually do the work. The second big goal is opening up the system via APIs to 3rd party companies and developers, so that they can build interesting applications on top of our system. Or connect it with something else, which was traditionally very difficult in hospitality industry. If you'd like to know more, check this sample [sales-pitch](https://www.facebook.com/MewsSystems/videos/351045845603219/) which explains what we offer to our clients and which problems we solve.

In the following sections, we'll try to answer frequently asked question that we believe a candidate might have when considering application to Mews:

- [Product](#product) - what we build.
- [Technology](#technology) - how we build it.
- [Teamwork](#teamwork) - how do we cooperate.
- [Job](#job) - offices, working hours, salaries.
- [Students](#students) - part-time opportunities for students.
- [Relocation](#relocation) - information for people outside CZ.
- [Media](#media) - Mews PR.

<a name='product'/>

### 🏨 Product 

- **What are the applications you are building?** The biggest part is the system for employees of hotels (receptionists, housekeepers, accountants, revenue managers, essentially everybody). For them, we also have native mobile applications with a subset of the functionality. Then we have web applicaton for guests of the hotels and booking widget that hotels can put to their websites. We also offer hotels free kiosk application that they can put into their lobby and guests can checkin there.
- **How do you handle if different hotels want different features?** We have some features that hotels can choose to enable or disable, according to their preferences. However, all of the various features are available to all hotels. Being SaaS, we have only one feature set and single live environment.
- **Does your system have public APIs?** Yes and it's one of its strengths. We try to take it really seriously and make it as user-friendly as possible for other developers to integrate with us. We also have a dedicated team to help the integration partners and give them advices how to best connect to us. So now, we have over 300 companies consuming our APIs. You can find out more here: https://mews-systems.gitbook.io/connector-api/
- **Do you produce any open source code?** Yes, some of the things we have done are now open sourced, but as it takes a lot of time to maintain, we try to be very careful with what we decide to make open. We decided that we'll be open sourcing mainly fiscalization libraries (like EET), becuase it's annoying having to implement it when some government decides to introduce it. You can check our public repositories here https://github.com/MewsSystems.

<a name='technology'/>

### 👨‍💻 Technology 

- **Which technologies do you use?** Generally, we try to use technology that is familiar to the development community. The benefit of using common technology is that everybody knows it well, and if not, they're able to Google the answers to their own questions. We also try to avoid building in-house solutions, but rather use third party sevices for responsibilities that are not directly related to our business (e.g. Logentries for logging, Sentry for error reporting, SendGrid for mailing). We want to focus on our product. Check our [Stackshare](https://stackshare.io/mews-systems/mews) to see a full list of what we use.
- **What is architecture of the backend?** The "executable" is a plain ASP.NET MVC application with no extra caveats. We use Entity Framework code-first approach, and we use Azure DB (a version of MSSQL) as our database. We run the application in Azure using App Services, so we don't need to manage the web servers or virtual machines. We use the original branches of .NET Framework and Entity Framework and are currently migrating to .NET Core. In general, there is data layer to access Azure DB, Azure Storage and Cosmos DB, business layer consisting of various components and a transactional layer (web, API, background jobs). BTW you can check our [Awesome Mews](https://github.com/MewsSystems/awesome-mews) reading list to see what is our philosophy, not only on backend.
- **How is frontend strutured?** We keep all our apps in one monorepo to share as much code as possible. All the apps are written on the same stack based on Redux, React and styled-components. We are migrating everything to TypeScript - current adoption is around one third of the codebase. We are fans of functional programming and apply many of its principles to our code. There is some legacy in some parts of the system still though from early days that we're moving away from, especially data heavy screens like reports are rendered on server using Razor templates.
- **How does technology stack for mobile apps look like?** Operator kiosk application is written from scratch in Kotlin. For Android development we use a standard set of libraries: Dagger, Retrofit, RxKotlin (and Detekt + ktlint for static code analysis). We have a fully set up CI/CD process for Operator app and are moving into this direction for other apps as well. iOS and Android versions of Mobile Commander were initially written in Swift and Java, but last year we've migrated them to Flutter.
- **Do you create microservices?** At the moment, no. The idea of having small units that are responsible for a specific task is great, however this architecture style would not match the current needs of our system. But with increasing size of the system and teams, we might evolve there naturally. However at the moment, we see modular monolith as the way for us going forward.
- **What is your test coverage?** We do not know. We do not measure code coverage, primarily because we do not see it as a crucial parameter. Of course, when the project started with just two people and not much time, they were not writing any tests. However, now that we have survived the startup stage, we have begun to rectify this situation and are now currently in the stage of covering of majority of the system with end-to-end integration API and UI tests. In our opinion, they bring the biggest value for cost.
- **How often do you deploy?** That depends on the application. All of our web and mobile applications are delivered continuously which means that when a pull request gets merged, a new version of the application is released. We aim for the same even on the backend application, but there it is a bit more difficult. Currently it is being released daily with daily feature-freezes. So even though it is not deployment per pull request, it is still pretty frequent.

<a name='teamwork'/>

### ⛹️ Teamwork 

- **Do you look for any specialists?** Not really. We hire "generic" developers and try to learn about their individual skills over time. Once we know what they enjoy working on, we try to embrace that and assign projects that each person is interested in. We have enough of a workload for the generic developers, which extends to all layers of our applications including new features and/or fixing bugs. At the moment, we have 8 teams with different responsibilities, so there are options to choose from.
- **Which team will I work in?** That's hard to tell in advance. Basically there are two factors that we take into account. First, we look at past experience and preferences of the candidate. Second, we look at our own needs that are based on company strategy. And combine those two inputs to find best match in one of 14 teams in the following groups:
    - B2B - Applications for hotels and their employees.
    - B2C - Applications for guests that visit the hotels.
    - Connectivity - APIs for 3rd parties.
    - Payments - Online payment solutions used by B2B and B2C applications.
    - Platform - Infrastructure, libraries, tooling etc. used by other teams.
- **How is your workload structured and prioritized?** From top-level perspective, if we would sum up all the issues in some time interval, we try to keep ratio 2:1 between features and consolidations. Consolidations are usually issues that originate in tech team like performance improvements, refactirong, elimination of technical debt etc. This applies to all teams, both platform and product teams.
- **Which versioning system do you use?** We host our code on GitHub, therefore we use git.
- **What is your branching workflow?** [Gitflow](https://nvie.com/posts/a-successful-git-branching-model/). On projects that are  continuously delivered, we have only master branch and feature branches, but that's just Gitflow without some branches.
- **Do you do code reviews?** Yes, for everything. Code review is a crucial part of how we work. We consider this a great practice, which helps the whole team understand what is happening in the system. As a developer, you not only gather feedback on your code (which helps you to improve), but you're also able to spread your knowledge and experience to others. It's also great platform for newcomers and juniors to ask questions and learn. For code-reviews we use Github pull requests. Currently, each pull request is reviewed by at least two people, one being senior.
- **Do you work with continuous integration?** Once you open a pull request, our CI server (Azure DevOps) picks it up automatically and runs checks against the changes. Unless all checks have passed, you will not be able to merge your pull request.
- **Do you have any code style rules?** We believe in naming things the right way rather than adding comments. We have our own set of code style rules (99% of .NET FW rules on backend, AirBnb on frontend) that are run during any build. If your code violates any of the rules, no one will be able to build the project. The goal is to have a uniform code style throughout the whole platform. We also create our own analyzers for common mistakes that appear during code-reviews.
- **How often do you schedule refactoring?** We do not schedule refactoring. We consider it to be an integral part of development work. When implementing a new feature, it is important to verify that all of the team code and architecture quality standards are applied. The fixes and features should all be done properly from the beginning.
- **How do you handle changes in the architecture of the system?** We try to introduce changes gradually while extending the capabilities of our system. For bigger changes, both on backend and frontend, we currently have "platform" teams whose only responsibility is to improve the system architecture, develop libraries and tools for other teams that are more focused on "product".
- **How many meetings would I have in a day?** That's up to team to decide, in most teams there is a short daily standup meeting. Each team also has an open weekly meeting (around 30 minutes), that anyone in the company can join to see what the team is currently up-to, what is planned for short-term future.
- **What is the company language?** In Prague office, where development is located, around 50% of people are czech and the rest is mix of many other nationalities. Therefore most of the meetings and communication is held in English. We also have native English speakers, some of them former English teachers in Prague, so we offer lessons for people who need to sharpen their language skills.

<a name='job'/>

### 🏢 Job

- **What does the office look like?** The office is located at [I.P. Pavlova square, 1789/5](https://www.google.com/maps/place/n%C3%A1m%C4%9Bst%C3%AD+I.+P.+Pavlova+1789%2F5,+120+00+Nov%C3%A9+M%C4%9Bsto/@50.0752762,14.4298992,3a,75y,184.78h,101.94t/data=!3m6!1e1!3m4!1s73pkxL_pvma3Oge6NEn3QQ!2e0!7i13312!8i6656!4m5!3m4!1s0x470b948c0ea2c643:0x3f011e15da2b48b!8m2!3d50.0749916!4d14.4298445), which is great in terms of transport options (metro line C, A, many trams).
- **Do you pay out special bonuses?** No, we do not. We prefer to pay good salaries transparently and on a regular basis so that you always know what you can count on.
- **Do you have stock option plan?** Yes, every employee is awarded certain number of depository receipts.
- **How many years would it take to reach a senior position?** We have a bit of a different understanding of what "senior developer" means. To us, a senior developer takes full responsibility for some part or component of the system and does not need anyone to help them with their tasks or to look over their shoulder. For this reason, we cannot guarantee that you will become a senior developer. It's up to you to determine how quickly you can become that skilled. Furthermore, if you consider yourself to be a senior developer already, it may happen that we will not agree. In our job posts for senior positions, you can find a list of topics. A senior developer should ideally have deeper knowledge with at least some of them.
- **Is it possible that a new junior developer would earn a higher salary than me as a senior developer?** No. We try to avoid frustrations about money and keep an eye on the salary of each person individually; we believe it should correspond to their skill level. Therefore, if we would desperately need to hire a junior developer with a salary higher than yours, we would increase all the salaries in the company so that the levels are kept intact. We want to pay salaries that are competitive to CZ market as we don't want you to leave because of a better salary offer. We don't have open salaries, but try to structure them as if they were.
- **Can I have a home office?** Yes, with the current world situation we became more open to possibilities to work from home. However, there are some rules to follow:
    - Home office is a thing of trust. We need to be able to trust that you are producing the same amount and quality of work as when working from the office. If you have a home office but do not respond for 2 hours on Slack, that's suspicious. Home office is not a day off.
    - When taking home office, we expect that you do not need anyone to help you accomplish your tasks that day and that you will not need to interrupt people via any communication channel.
- **What are the working hours?** That really depends on you. Every person has a different rhythm to their life. As long as you accomplish what you have promised to accomplish, it's up to you. Most people are in the office between 11:00 and 17:00. Although there is a lot of flexibility, your working hours should intersect with this time period so that you have opportunities to meet with the team and consult them regarding your work.

<a name='students'/>

### 🎓 Students

- **Is it possible to work for Mews while studying?** Yes, we have many students in the team, especially from [MFF CUNI](https://www.mff.cuni.cz/en) and [CTU](https://www.cvut.cz/en). We treat students just like any other employees, which means they're working on real projects, in product teams with other developers and getting market salary.
- **Do you provide any support for students?** Because Mews development team was founded by a group of students that met in school and were still studying, we know what it means to both work and study. Therefore we're OK with you having a month break during exam periods, so that you can prepare for the exams. Students also have lot of time flexibility because of their school schedules. And since we have graduates from both of the aforementioned schools in our team, we can also help with the studying.
- **Can I do a bechelor or master thesis for Mews?** Definitely, we have many ideas that are appropriate either for bachelor or master theses. Just reach out to us and we can discuss the possibilities.

<a name='relocation'/>

### ✈️ Relocation 

- **Do you help with relocation?** Yes, we do. We have an agency that would contact you to help you resolve any paperwork you might need for both our government and your own. We also provide accommodation for the first few months until you are able to find your own apartment. We have "Mews flat" in Prague and London which we manage using our own system.
- **What is the cost of living in Prague?** You can find it and compare with other cities at https://www.numbeo.com/cost-of-living/in/Prague.
- **How long does it take to relocate?** Unfortunately, it takes a lot of time; usually around 6 months. First, we must submit the the job posting announcing your position to the Czech Labour office, where it must be pending publicly for 30 days for potential employees. After that, the paperwork is processed, and both governments have 30-60 days to finish the procedure. Regrettably, these offices like to take their time and therefore, it is difficult to give an exact estimate about how long it takes from start to finish.

<a name='media'/>

### 📰 Media

For 7 years we went rather unnoticed (mainly in our home base - Czech Republic) and didn't really mind as we never focused on clients in Czech Republic (only 5% of all). But things are changing...

- **English**
    - [TechCrunch](https://techcrunch.com/2019/08/29/mews-grabs-33m-series-b-to-modernize-hotel-administration/) - announcement of the Series B investment.
    - [HospitalityTech](https://www.hospitalitynet.org/news/4096068.html) - Mews' acquisition of Planet Winner PMS.
    - [Travel Tech Conference Russia](https://www.youtube.com/watch?v=EvZSfzKccC0) - Richard (founder) talking about why hotels should think like tech companies.
    - [TechUncovered](https://www.youtube.com/watch?v=WQ90qs8iA30) - First #TechUncovered event organized by us with cooperation with productboard, STRV, SCS Software and Pipedrive. CTO's, founders and tech leads discussing scaling tech teams.
- **Czech**
    - [CzechCrunch podcast](https://www.czechcrunch.cz/2019/12/cesti-jednorozci-v-cekarne-zakladatele-mews-a-productboardu-v-czechcrunch-podcastu-o-budovani-miliardovych-firem/) - Intervirew with the founder of Mews Richard Valtr and CEO of Productboard Hubert Palan.
    - [Forbes podcast](https://www.forbes.cz/stres-je-nezadouci-a-omluva-pro-neznalost-neexistuje-rika-v-podcastu-filozof-ktery-meni-hotelnictvi/) - Another philosophical podcast featuring founder Richard.
    - [Lupa.cz](https://www.lupa.cz/clanky/jan-siroky-mews-na-nasem-softwaru-funguje-asi-tisic-hotelu-hilton-zatim-nemame/) - technical interview with the CTO Honza.
    - [Forbes](https://www.forbes.cz/velka-rana-nenapadny-cesky-startup-mews-ziskava-investici-768-milionu-korun/) - announcement of the Series B investment.
    - [CzechCrunch](https://www.czechcrunch.cz/2019/08/snil-o-psani-a-filmarine-dnes-richard-valtr-po-celem-svete-stavi-superhotely-a-buduje-miliardovy-startup-mews/) - interview with the founder Richard about his visions behind Mews.
    - [E15](https://www.e15.cz/byznys/budoucnost-byznysu/jejich-aplikaci-vyuziva-pres-tisic-svetovych-hotelu-ted-cesti-mews-dobyvaji-usa-1365579) - changing the hospitality industry.
    - [Radiozurnal](https://radiozurnal.rozhlas.cz/spolupraci-firem-a-vysokych-skol-si-pochvaluji-obe-strany-partneri-jsou-ale-take-8096412) - discussion about why it's important to support Universities.
    - [Computerworld](https://computerworld.cz/udalosti/jak-to-chodi-v-uspesnem-start-upu-55815) - Interview with CPO Jirka about how it is going in a growing start-up.
