import React, {Component} from 'react'; //je potřeba aby fungovala 'jsx' syntaxe
import ReactDOM from 'react-dom'; //vstupní bod pro renderování react
import CurrencyPairsSelector from './currency-paris-selector';
import CurrencyPairsRate from './currency-pair-rate-list';
import { endpoint, interval, server, mainColor } from '../config';
import ReactLoading from 'react-loading';
import styles from '../styles/loader.css';

class AppLayout extends Component {

    constructor(props){
        super(props);
        this.state = {
            exchangeRates: [],
            isLoaded: false,
        }
    }
    render() {

        var { isLoaded, items } = this.state;
        //if(!isLoaded) {
            return <ReactLoading color={mainColor} type="spin" className={styles.loading}/>
       /* }
        return <div>
            <CurrencyPairsSelector />
            <CurrencyPairsRate />
        </div>;*/ 
    }
    componentDidMount(){
        fetch(`${server}/configuration`)
        .then(res => res.json())
        .then(json => {
            
            this.setState({
                isLoaded: true,
                exchangeRates: json,
            });
            console.log(this.state);
        })
    }
} 

export default AppLayout;