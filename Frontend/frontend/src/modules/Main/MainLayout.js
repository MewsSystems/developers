import React from 'react';

import Header from './components/Header/Header';
import Container from '../../atoms/Container/Container';
import RateListPage from '../RateList/pages/RateListPage';

import StyledMainLayout from './components/MainLayout/StyledMainLayout';


const MainLayout = () => (
  <StyledMainLayout>

    <Header />

    <Container>
      <div className="mainLayout--body">
        <RateListPage />
      </div>
    </Container>

  </StyledMainLayout>
);


export default MainLayout;
