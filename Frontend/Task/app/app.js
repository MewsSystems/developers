import List from './List'
import Selector from './Selector'

const e = React.createElement

class App extends React.Component {
    constructor(props){
        super(props)
        this.state= {
            configuration: {
                "currencyPairs": {
                  "70c6744c-cba2-5f4c-8a06-0dac0c4e43a1": [
                    {
                      "code": "AMD",
                      "name": "Armenia Dram"
                    },
                    {
                      "code": "GEL",
                      "name": "Georgia Lari"
                    }
                  ],
                  "41cae0fd-b74d-5304-a45c-ba000471eabd": [
                    {
                      "code": "PHP",
                      "name": "Philippines Peso"
                    },
                    {
                      "code": "DZD",
                      "name": "Algeria Dinar"
                    }
                  ],
                  "5b428ac9-ec57-513d-8a08-20199469fb4d": [
                    {
                      "code": "LSL",
                      "name": "Lesotho Loti"
                    },
                    {
                      "code": "AUD",
                      "name": "Australia Dollar"
                    }
                  ],
                  "f816e384-0e43-5ce7-a017-deaa8d666774": [
                    {
                      "code": "BDT",
                      "name": "Bangladesh Taka"
                    },
                    {
                      "code": "VEF",
                      "name": "Venezuela Bolivar"
                    }
                  ],
                  "5b98842f-bfe5-5564-b321-068763d7e2a3": [
                    {
                      "code": "QAR",
                      "name": "Qatar Riyal"
                    },
                    {
                      "code": "JOD",
                      "name": "Jordan Dinar"
                    }
                  ],
                  "a2bda952-4369-5d41-9d0b-e6c9947e5b82": [
                    {
                      "code": "SRD",
                      "name": "Suriname Dollar"
                    },
                    {
                      "code": "CZK",
                      "name": "Czech Republic Koruna"
                    }
                  ],
                  "1993f7b9-f9be-551a-beac-312d6befd0cf": [
                    {
                      "code": "CZK",
                      "name": "Czech Republic Koruna"
                    },
                    {
                      "code": "LSL",
                      "name": "Lesotho Loti"
                    }
                  ],
                  "61fb0e0d-626e-5e0a-831a-ef95d5c32203": [
                    {
                      "code": "DZD",
                      "name": "Algeria Dinar"
                    },
                    {
                      "code": "MVR",
                      "name": "Maldives (Maldive Islands) Rufiyaa"
                    }
                  ],
                  "b7fdd67f-5051-58b7-a3c6-84f5da637df5": [
                    {
                      "code": "BIF",
                      "name": "Burundi Franc"
                    },
                    {
                      "code": "CLP",
                      "name": "Chile Peso"
                    }
                  ],
                  "611398c5-6bd9-596e-8803-3ed0b093995d": [
                    {
                      "code": "HKD",
                      "name": "Hong Kong Dollar"
                    },
                    {
                      "code": "FKP",
                      "name": "Falkland Islands (Malvinas) Pound"
                    }
                  ]
                }
              },
            rates:{},
            filter: ""
        }

        this.onFilterChange = this.onFilterChange.bind(this)
    }

    componentDidMount() {
        //Call Api 
        this.getConfiguration()
        //Set State

        //Setinterval
        // setInterval( this.getRates, 5000)
    }

    getConfiguration() {
        let json =  fetch('/configuration', {
            method: 'GET',
            headers: {'Content-Type': 'application/json'}
        }).then(function(res) {
            console.log("RES", res)
            return res
        }).then(function(json){
            console.log("JSON", json)
            return json
        })
        return json
    }

    onFilterChange(filter){
      console.log("OFC", filter)
      this.setState({
        filter: filter
      })
    }

    render() {
        return e('div', null, [
            e('h1', {key: 'title'}, 'Currrency App'),
            e(Selector, {key:'selectorcomponent', filterUpdate: this.onFilterChange}),
            e(List, {key:'listcomponent',configuration: this.state.configuration, filter: this.state.filter}),
        ])
    }
}


ReactDOM.render(e(App), document.querySelector('#exchange-rate-client'))