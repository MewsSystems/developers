# Movie Search Engine

Hello everyone, this is very simple Movie search engine. The design isn't the top quality as it's not my expertize, but I hope you will find more beauty in the code itself.

The easiest way to test it:
```bash
git clone git@github.com:fabulator/developers.git
cd developers/Frontend
npm ci
npm run develop
```

I didn't use React Create App because I wanted to keep it clean and simple in terms of codebase. I use my own eslint config to keep nice code and typescript validator to ensure that typings are correct.

```bash
npm run lint
npm run tsc
```

For the API calls I use my extension of fetch (rest-api-handler), other libraries are pretty casual. 
