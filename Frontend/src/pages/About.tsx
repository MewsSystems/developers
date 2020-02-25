import React from 'react'
import Page from '../layout/Page'
import Container from '../layout/Container'
import styled from '../utils/styled'

const About = () => {
  return (
    <Page>
      <Container>
        <PageContent>
          <h1>Welcome!</h1>
          <p>
            Welcome to the Redux 4.0.7 + TypeScript 3.8.2 example! This example site shows you the ideal project structure, recommended
            libraries, as well as design pattern on writing type-safe React + Redux app with TypeScript.
          </p>
          <p>
            This project is intended as a test task for position of a Frontend Developer at{' '}
            <a href="https://github.com/MewsSystems/developers" target="blank" rel="noopener noreferrer">
              MewsSystems
            </a>
            .
          </p>
          <p>
            To demonstrate it, I created a website which pulls data from the{' '}
            <a href="https://developers.themoviedb.org/3/getting-started/introduction" target="blank" rel="noopener noreferrer">
              The MovieDB API
            </a>
            , via search box and display a information about movies. This will also demonstrate how to structure your stores for each
            feature/module in a Redux-enabled app.
          </p>
          <p>Enjoy your stay!</p>
        </PageContent>
      </Container>
    </Page>
  )
}

export default About

const PageContent = styled('article')`
  max-width: ${props => props.theme.widths.md};
  margin: 0 auto;
  line-height: 1.6;

  a {
    color: ${props => props.theme.colors.brand};
  }

  h1,
  h2,
  h3,
  h4 {
    margin-bottom: 0.5rem;
    font-family: ${props => props.theme.fonts.headings};
    line-height: 1.25;
  }
`
