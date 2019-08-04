// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import ListItem from './CoursesListItem';

const List = styled.ul`
  list-style-type: none;
`;

const CoursesList = () => (
  <List>
    <ListItem currencyPair="ABC/DEF" courses={[1.5, 2.5]} />
    <ListItem currencyPair="ABC/DEF" courses={[1.5]} />
    <ListItem currencyPair="ABC/DEF" courses={[2.5, 1.5]} />
    <ListItem currencyPair="ABC/DEF" courses={[2.5, 2.5]} />
    <ListItem currencyPair="ABC/DEF" courses={[]} />
  </List>
);

export default CoursesList;
