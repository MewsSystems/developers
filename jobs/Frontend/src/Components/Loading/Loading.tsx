import React, { ReactNode } from 'react';
import { StatusTitle } from '../StatusTitle/StatusTitle';
import { LoadingElement } from './Loading.styled';

interface LoadingProps {
  children: ReactNode;
  status: 'idle' | 'pending' | 'fulfilled' | 'rejected';
}

export const Loading = (props: LoadingProps) => {
  const { status, children } = props;

  const pageBody = () => {
    switch(status) {
      case 'pending':
        return <LoadingElement><StatusTitle>Loading...</StatusTitle></LoadingElement>;
      case 'fulfilled':
        return children;
      case 'rejected':
        return <StatusTitle>There has been an error while loading the resource</StatusTitle>
    }
  }

  return <>{pageBody()}</>
};
