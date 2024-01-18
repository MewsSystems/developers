import Head from 'next/head'
import React from 'react';
import SearchLayout from '@/Layouts/Search';

export default function Home() {
  return (
    <>
      <Head>
        <title>Search Movies</title>
      </Head>
      <SearchLayout/>
    </>
  )
}
