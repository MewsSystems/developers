import React, { Component } from 'react';
import { ToastContainer } from 'react-toastify';

import styles from './App.module.css';
import 'react-toastify/dist/ReactToastify.css';

import Rates from './features/Rates';

class App extends Component {
  render() {
    const {
      config: { interval },
    } = this.props;

    return (
      <>
        <main className={styles.app}>
          <Rates interval={interval} />
        </main>
        <footer>
          <ToastContainer />
        </footer>
      </>
    );
  }
}

export default App;
