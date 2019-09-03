const e = React.createElement

export default class List extends React.Component {
    constructor(props){
        super(props)
        this.state = {
            configuration: [],
            rates: []
        }
    }

    componentDidMount(){
        console.log("PROPS", this.props.configuration.currencyPairs["70c6744c-cba2-5f4c-8a06-0dac0c4e43a1"]
        )
        let pairs = this.props.configuration.currencyPairs
        let configuration = []
        let rates = []
        for( var pair in pairs){
            console.log("CDM", pair )
            configuration.push(pairs[pair])
        }
        this.setState({
            configuration: configuration,
            rates: rates
        })
        setInterval(function() {
            for( var pair in pairs){
                console.log("CDM", pair )
                rates.push(getRate(pair))
            }
        }, 5000)
        // console.log(configuration)
        

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
            console.log("RES", res)
            return res
        }).then(function(json){
            console.log("JSON", json)
            json.rates[id]
            return json.rates[id]
        })
        return rate

    }


    pairMatchesFilter(pair) {
        const filter =  this.props.filter
        console.log("filter", filter)
        console.log("name0",pair[0].name)
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
                            e('th', null, 'Trend '),

                            ),
                    ),
                    e('tbody',null, this.state.configuration.map(pair => {
                        count++
                        let matches = this.pairMatchesFilter(pair)
                        console.log("PAIR", pair)
                         if(matches)  {
                               return  e('tr', null, 
                            [   
                                e('td', {key: pair[0].name+'/'+pair[1].name},pair[0].name+'/'+pair[1].name),
                                e('td', {key: pair[0].name}, this.state.rates[count]),
 
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

