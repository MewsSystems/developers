import React, { Component } from 'react';
import Axios from 'axios';

export default class CurrencyList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      apiConfiguration: [],
    };
    this.apiConfigurationDownload();
  }

  apiConfigurationDownload() {
    console.log('run');
    Axios.get('http://localhost:3000/configuration', { timeout: 10000 }).then(
      response => {
        const apiConfiguration = Object.entries(response.data.currencyPairs);
        console.log('RUN', apiConfiguration);
        this.setState({
          apiConfiguration,
        });
      }
    );
  }

  render() {
    const { apiConfiguration } = this.state;
    console.log('render');
    const currencyCouples = apiConfiguration.map((key, i) => {
      return (
        <li className="list-group-item" key={key[0]}>
          <p>element: {i}</p>
          <p>code1: {key[1][0].code}</p>
          <p>name1: {key[1][0].name}</p>
          <p>code2: {key[1][1].code}</p>
          <p>name2: {key[1][1].name}</p>
        </li>
      );
    });

    return <ul className="list-group">{currencyCouples}</ul>;
  }
}

// const ScreenKeyboard = props => {
//   const keys = mapScreenKeyboard.map((key, i) => {
//     return (
//       // eslint-disable-next-line react/no-array-index-key
//       <div className="grid-item" key={i}>
//         <a href={key.link} target="_blank" rel="noopener noreferrer">
//           <button
//             type="submit"
//             className="keyboard-button"
//             value={key.value}
//             onClick={() => props.onInputChange(key.value)}
//           >
//             <h4 className="keyboard-display1">{key.display1}</h4>
//             <p className="keyboard-display2">{key.display2}</p>
//             <i className={key.icon} />
//           </button>
//         </a>
//       </div>
//     );
//   });
//   return <div className="grid">{keys}</div>;
// };
