import styled, { css } from "styled-components";
import { ThStyles, TrStyles } from "../../css/component-styles.js";
import { endpoint } from "../config.json";


const Th = styled.th`${ThStyles}`;
const Tr = styled.tr`${TrStyles}`;

class RateEntry extends React.Component {
    render() {
        let curValueFloat  = parseFloat(this.props.curValue);
        let prevValueFloat = parseFloat(this.props.prevValue);
        
        // Rate stagnating
        let chr = "▬";
        let trendType = "trendStagnating";

        // Rate going up
        if (curValueFloat > prevValueFloat) {
            chr = "▲";
            trendType = "trendGrowing";
        }
        // Rate going down
        else if (curValueFloat < prevValueFloat){
            chr = "▼";
            trendType = "trendDeclining";
        }

        return (
            <Tr className="RateList">
                <Th className="RateList">{this.props.from}/{this.props.to}</Th>
                <Th className="RateList">{this.props.curValue}</Th>
                <Th className="RateList"><span className={trendType}>{chr}</span></Th>
            </Tr>
        );
    }
}

export class RateList extends React.Component {
    constructor(props) {
        super(props);

        // Binding methods so that we can call them from
        // inside other methods.
        this.setRates    = this.setRates.bind(this);
        this.updateRates = this.updateRates.bind(this);

        this.state = Object.freeze({
            pairs: null,
            rates: null,
            prevRates: null
        });
    }

    setPairs(newPairs) {
        // Update the state with the new pairs.
        this.setState({pairs: newPairs});

        // Once we have the pairs, make the first request for rates.
        this.updateRates();
    }

    setRates(newRates) {
        // For some reason sometimes setRates is called with empty json.
        if (Object.getOwnPropertyNames(newRates).length === 0) {
            return;
        }

        // We want to check if some of the currency pairs suddenly
        // were removed from the server.
        let newPairs = {};
        for (let id in newRates) {
            newPairs[id] = this.state.pairs[id];
        }
        this.setState({pairs: newPairs});


        // If this is not the first update of rates
        if (this.state.prevRates) {
            // We want to change previous rates only if a change in 
            // current rate appeared at the moment of the call (so that
            // if this method is called when the rates didn't update yet,
            // we don't set the prevRates to a copy of current rates).
            let prevRatesUpdated = {};
            for (let id in newRates) {
                // If nothing changed on this iteration.
                if (newRates[id] == this.state.rates[id]) {
                    prevRatesUpdated[id] = this.state.prevRates[id];
                }
                // If a change was detected.
                else {
                    prevRatesUpdated[id] = this.state.rates[id];
                }
            }

            // Updating previous and current rates.
            this.setState({prevRates: prevRatesUpdated, rates: newRates});
        }
        // If this is the first update of rates
        else {
            this.setState({prevRates: newRates, rates: newRates});
        }
    }

    updateRates() {
        let ids = [];
        for (let id in this.state.pairs) {
            ids.push(id);
        }

        axios.get(`${endpoint}/rates`, {
                timeout: 500,
                params: {
                    currencyPairIds: ids
                }
            })
            .then((response) => {
                this.setRates(response.data.rates);
            })
            .catch((error) => {
                console.log(`Error while requesting rates: (${error}) Requesting again...`);
                setTimeout(this.updateRates, 500);
            });
    }
    

    render() {
        // If we didn't get the currency pairs yet:
        if (!this.state.pairs) {
            return null;
        }

        // Otherwise:
        else {
            // Going through all currency pairs.
            let pairs = [];
            for (let id in this.state.pairs) {
                // If the pair is not checked, skipping it.
                if (!this.props.isPairSelected(id)) {
                    continue;
                }

                // Fetching the shortcut names.
                let from = this.state.pairs[id][0].code;
                let to   = this.state.pairs[id][1].code;

                // If we didn't get the rates yet, they will not be displayed.
                let cur  = "Updating...";
                let prev = "Updating...";
                if (this.state.rates && this.state.rates[id]) {
                    cur  = this.state.rates[id];
                    prev = this.state.prevRates[id];
                }

                // Making a new rate component with the needed values.
                pairs.push(<RateEntry key={id} from={from} to={to} curValue={cur} prevValue={prev} />);
            }

            return pairs;
        }
    }
}
