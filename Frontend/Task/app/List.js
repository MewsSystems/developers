const e = React.createElement

export default class List extends React.Component {
    constructor(props){
        super(props)
        this.state = {
            configuration: [],
            rates: []
        }
        this.interval = this.interval.bind(this)
    }
    
    componentDidMount(){
        // console.log("PROPS", this.props.configuration.currencyPairs["70c6744c-cba2-5f4c-8a06-0dac0c4e43a1"])
        let pairs = this.props.configuration.currencyPairs
        let configuration = []
        let rates = []
        let count = -1
        for( var pair in pairs){
            count ++
            // console.log("CDM", pair )
            configuration.push(
                {id:pair, value:pairs[pair] ,rate: Math.random(), state:"stagnating"} //normally this would be "rate: this.getRate(pair)"
            )
            rates.push(count)
        }
        this.setState({
            configuration: configuration,
            rates: rates
        },() => {
            setInterval(this.interval,  1000)
        })
        
        // console.log(configuration) //TODO: compare rats and give state
        

    }


    interval() {
        let rates = this.state.rates
        let count = -1
        console.log("RATES", rates)
        if(rates !== []){
            count++
            let conf = this.state.configuration
            conf.map(pair => {  
                let currentRate  =  Math.random()  //This normally would call to getRates(pair.id)
                pair.state = this.calculateTrend(pair.rate, currentRate)
                print.rate = currentRate
                console.log(pair.rate)
            } )
            this.setState({
                configuration: conf, rates: rates
            }, () => {
               console.log( "CONFIG", conf)
            })
        }
    }

    getRate(id){
        const contentBody = 
            {
                currencyPairIds: [ id ]
            }
        
        let rate =  fetch('/rates', {
            method: 'GET',
            body: JSON.stringify(contentBody),
            headers: {'Content-Type': 'application/json'}
        }).then(function(res) {
            // console.log("RES", res)
            return res
        }).then(function(json){
            // console.log("JSON", json)
            json.rates[id]
            return json.rates[id]
        })
        return rate

    }

    calculateTrend(previousTrend, currentTrend){
        if(previousTrend < currentTrend) return "growing"
        if(previousTrend === currentTrend) return "stagnating"
        if(previousTrend > currentTrend) return "declining"
    }

    pairMatchesFilter(pair ) {
        const filter =  this.props.filter
        // console.log("filter", filter)
        // console.log("name0",pair[0].name)
        var match0 =  pair[0].name.match(filter)
        var match1 =  pair[1].name.match(filter)
        if(filter == "" || (match0 || match1)){
            
            return true
        }else{
           return false
        }
    }

    render() {
        var count = -1
        return e('div', {key:'divlist'}, [
            e('h1', {key: 'byeee'},  
            e(
                'table',null,
                    e('thead',null,
                        e('tr', null,
                            e('th', null, 'Name'),
                            e('th', null, 'Rate'),
                            e('th', null, 'Trend '),

                            ),
                    ),
                    e('tbody',null, this.state.configuration.map(pair => {
                        let couple = pair.value
                        // console.log(couple)
                        count++
                        let matches = this.pairMatchesFilter(pair.value)
                        // console.log("PAIR", pair)
                         if(matches)  {
                               return  e('tr', null, 
                            [   
                                e('td', {key: couple[0].name+'/'+couple[1].name},couple[0].name+'/'+couple[1].name),
                                e('td', {key: couple[0].name},  pair.rate),
                                e('td', {key: couple[0].name},  pair.state),
 
                            ]
                             )
                        }
                    })

                    )
                )
            )
        ])
    }
}

