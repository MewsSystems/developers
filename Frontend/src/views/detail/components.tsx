import * as React from 'react';

type MovieFactProps = {
  label: string,
  value?: React.ReactNode
}

export const Fact: React.FC<MovieFactProps> = ({ label, value }) => {

  if (!value) {
    return null;
  }

  return (
    <div style={{ display: 'flex' }}>
      <div style={{ width: '200px' }}>{label}</div>
      <div>{value}</div>
    </div>
  )
}

type MoviePosterProps = {
  fileName: string;
};

export const Poster: React.FC<MoviePosterProps> = ({ fileName }) => {
  return <img
    src={`https://image.tmdb.org/t/p/w185/${fileName}`}
    style={{ height: '100%' }} width="185" alt="" />
}

type MoveeCompanyInfoProps = {
  name?: string,
  logo_path?: string | null,
};

export const CompanyInfo: React.FC<MoveeCompanyInfoProps> = ({ logo_path, name }) => {
  return (
    <>
      <div>{name}</div>
      {logo_path && <img src={'https://image.tmdb.org/t/p/w185/' + logo_path} />}
    </>
  )
}
