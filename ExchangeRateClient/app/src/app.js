import { endpoint, interval } from './config';


class Rate extends React.Component {
    render() {
        let cur_float  = parseFloat(this.props.cur_value);
        let prev_float = parseFloat(this.props.prev_value);
        
        // Rate stagnating
        let chr = "▬";
        let trend_type = "trend_stagnating";

        // Rate going up
        if (cur_float > prev_float) {
            chr = "▲";
            trend_type = "trend_growing";
        }
        // Rate going down
        else if (cur_float < prev_float){
            chr = "▼";
            trend_type = "trend_declining";
        }

        return (
            <tr>
                <th>{this.props.from}/{this.props.to}</th>
                <th>{this.props.cur_value}</th>
                <th><span className={trend_type}>{chr}</span></th>
            </tr>
        );
    }
}

class RatesList extends React.Component {
    constructor(props) {
        super(props);

        // Binding methods so that we can call them from
        // inside other methods
        this.set_rates = this.set_rates.bind(this);
        this.update_rates = this.update_rates.bind(this);

        this.state = {
            pairs: null,
            rates: null,
            prev_rates: null
        };
    }

    set_pairs(json) {
        // Update the state with the new pairs.
        this.setState({pairs: json});

        // Once we have the pairs, make the first request for rates.
        this.update_rates();
    }

    set_rates(json) {
        // For some reason sometimes set_rates is called with empty json.
        if (jQuery.isEmptyObject(json)) {
            return;
        }

        console.log("Rates set!");
        
        // We want to check if some of the currency pairs suddenly
        // were removed from the server.
        let new_pairs = {}
        for (let id in json) {
            new_pairs[id] = this.state.pairs[id];
        }
        this.setState({pairs: new_pairs});


        // If this is not the first update of rates
        if (this.state.prev_rates) {
            // We want to change previous rates only if a change in 
            // current rate appeared at the moment of the call (so that
            // if this method is called when the rates didn't update yet,
            // we don't set the prev_rates to a copy of current rates).
            let prev_rates = {};
            for (let id in json) {
                // If nothing changed on this iteration.
                if (json[id] == this.state.rates[id]) {
                    prev_rates[id] = this.state.prev_rates[id]
                }
                // If a change was detected.
                else {
                    prev_rates[id] = this.state.rates[id];
                }
            }

            // Updating previous and current rates.
            this.setState({prev_rates: prev_rates, rates: json});
        }
        // If this is the first update of rates
        else {
            this.setState({prev_rates: json, rates: json});
        }
    }

    update_rates() {
        let ids = []
        for (let id in this.state.pairs) {
            ids.push(id);
        }

        $.ajax({
            method: "GET",
            url: endpoint + "rates",
            dataType: "json",
            timeout: 500,
            data: {currencyPairIds: ids},
            success: (json) => {
                this.set_rates(json.rates);
            },
            error: (request, status, err) => {
                console.log("Error while requesting rates! Requesting again...");
                setTimeout(this.update_rates, 500);
            }
        });
    }
    

    render() {
        // If we didn't get the currency pairs yet:
        if (!this.state.pairs) {
            return (<tr><th>Fetching currency pairs...</th></tr>);
        }

        // Otherwise:
        else {
            // Going through all currency pairs.
            let pairs = []
            for (let id in this.state.pairs) {
                // If the pair is not checked, skipping it.
                if (!this.props.get_pair_state(id)) {
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
                    prev = this.state.prev_rates[id];
                }

                // Making a new rate component with the needed values.
                pairs.push(<Rate key={id} from={from} to={to} cur_value={cur} prev_value={prev} />);
            }

            return pairs;
        }
    }
}


class RateSelector extends React.Component {
    render() {
        let class_name = this.props.checked ? "checked" : "unchecked";
        return (
            <tr className={class_name}><th>
                <div className="checkbox">
                    <input type="checkbox" name={this.props.name} id={this.props.id} 
                           checked={this.props.checked} onChange={this.props.onChange} />
                    <label htmlFor={this.props.id}> {this.props.name} </label>
                </div>
            </th></tr>
        );
    }
}

class RateSelectorList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            names: null,
            is_checked: new Map()
        }

        this.handleChange = this.handleChange.bind(this);
        this.get_state    = this.get_state.bind(this);
    }

    set_names(json) {
        let names = {};
        let is_checked = new Map();

        // If the selection is stored in cookies, get it.
        let selection = null;
        if (Cookies.get('selection')) {
            selection = JSON.parse(Cookies.get('selection'))
        }

        for (let id in json) {
            names[id] = json[id][0].code + "/" + json[id][1].code;

            is_checked[id] = true;
            if (selection && !(typeof selection[id] === "undefined")) {
                is_checked[id] = selection[id];
            }
        }

        this.setState({names: names, is_checked: is_checked});
    }

    get_state(id) {
        return this.state.is_checked[id];
    }

    handleChange(e) {
        const id = e.target.id;
        let new_checked = this.state.is_checked;

        new_checked[id] = !this.state.is_checked[id];
        this.setState({is_checked: new_checked});

        // Update function passed from parent ExchangeRateClient class.
        this.props.redraw_rate_list();

        // Save current state to cookies.
        Cookies.set('selection', JSON.stringify(this.state.is_checked));
    }

    render() {
        if (!this.state.names) {
            return (<tr><th>Fetching currency pairs...</th></tr>);
        }

        let checkboxes = [];
        for (let id in this.state.names) {
            let name = this.state.names[id];
            let checked = this.state.is_checked[id];
            checkboxes.push(<RateSelector key={id} id={id} name={name} 
                                          checked={checked} onChange={this.handleChange} />);
        }

        return checkboxes;
    }
}

class ExchangeRateClient extends React.Component {
    constructor(props) {
        super(props);

        this.rate_selector_list = React.createRef();
        this.rate_list          = React.createRef();
    }

    componentDidMount() {
        this.request_configuration();
    }

    request_configuration = () => {
        console.log("Configuration request");
        
        // Loading previous configuration, if it is stored in cookies.
        if (Cookies.get('configuration')) {
            let json = JSON.parse(Cookies.get('configuration'));
            this.rate_list.current.set_pairs(json.currencyPairs);
            this.rate_selector_list.current.set_names(json.currencyPairs);
            console.log("Restored configuration from cookies.");
        }


        $.ajax({
            method: "GET",
            url: endpoint + "configuration",
            dataType: "json",
            
            // Timeout is set to 4.5s in order to show the functionality 
            // of the error handler (the server gives timeouts in range
            // from 3s to 5s, so the error will be handled quite often).
            // I'm not sure if I really needed to implement it, though.
            timeout: 4500,
            success: (json) => {
                // If not all components are defined yet.
                if (!this.rate_list || !this.rate_selector_list) {
                    console.log("Not all child components generated yet!");
                    setTimeout(this.request_configuration, 100);
                    return;
                }

                console.log("Currency pairs request success:");
                console.log(json);

                // Saving the received configuration to cookies.
                Cookies.set('configuration', JSON.stringify(json));

                // Updating pairs for rates list and repeatedly requesting rates.
                this.rate_list.current.set_pairs(json.currencyPairs);
                setInterval(this.rate_list.current.update_rates, interval);

                // Updating names for the pair selector, too.
                this.rate_selector_list.current.set_names(json.currencyPairs);
            },
            error: (request, status, err) => {
                console.log("Error while requesting currency pairs! Requesting again...");
                setTimeout(this.request_configuration, 500);
            }
        });
    }

    redraw_rate_list = () => {
        this.rate_list.current.forceUpdate();
    }

    get_pair_state = (id) => {
        // Is the given pair selected?
        if (!this.rate_selector_list.current) return true;
        return this.rate_selector_list.current.get_state(id);
    }

    render() {
        return (
            <div>
                <div id="rates-selector">
                    <table className="selector-table">
                        <tbody>
                            <RateSelectorList ref={this.rate_selector_list} 
                                              redraw_rate_list={this.redraw_rate_list}/>
                        </tbody>
                    </table>
                </div>
                <div id="rates-list">
                    <table className="rates-table">
                        <tbody>
                            <RatesList ref={this.rate_list} get_pair_state={this.get_pair_state}/>
                        </tbody>
                    </table>
                </div>
            </div>
        );
    }
}


ReactDOM.render(
    <ExchangeRateClient />,
    document.getElementById('exchange-rate-client')
);
