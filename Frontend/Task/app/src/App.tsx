import React from 'react'
import {connect} from 'react-redux'
import RatesList from './components/rates-list/rates-list.component';
import Header from './components/header/header.component'
import './App.css'

const App: React.FC = () => {
  return(
    <div>
      <Header/>
      <RatesList/>
    </div>
  )
}

export default App