import React from 'react'
import RatesList from './components/RatesList/rates-list.component';
import Header from './components/Header/header.component'

const App: React.FC = () => {
  return(
    <div>
      <Header/>
      <RatesList/>
    </div>
  )
}

export default App;