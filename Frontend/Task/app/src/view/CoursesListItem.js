// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import Text from '../components/Text';
import Trend from './CourseTrend';

type Props = {|
  +currencyPair: string,
  +courses: number[],
|};

const Container = styled.li`
  padding-bottom: 8px;
`;

const CoursesListItem = ({ currencyPair, courses }: Props) => {
  const recentCourses = courses.slice(-2);

  const currentCourse = recentCourses[recentCourses.length > 1 ? 1 : 0];
  const previousCourse = recentCourses.length === 2 ? recentCourses[0] : null;

  return (
    <Container>
      <Text bold>
        {currencyPair}: <Text element="span">current course:</Text>{' '}
        {currentCourse ? (
          <Text element="span" bold>
            {currentCourse}, <Trend currentCourse={currentCourse} previousCourse={previousCourse} />
          </Text>
        ) : (
          <Text element="span">unknown</Text>
        )}
      </Text>
    </Container>
  );
};

export default CoursesListItem;
