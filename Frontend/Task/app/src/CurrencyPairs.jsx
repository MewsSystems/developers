//import { endpoint, interval } from './config';

class CurrencyPairs extends React.Component {
    constructor(props) {
      super(props);
      this.state = {
        error: null,
        isLoaded: false,
        items: []
      };

      this.currencyPairs = [];
      this.endpoint = props.endpoint.replace("/rates", "");
    }

    componentDidMount() {
      var me = this;
      fetch(me.endpoint + "/configuration")
        .then(res => res.json())
        .then(
          (result) => {
            for (var item in result.currencyPairs) {
              me.currencyPairs.push({id: item, pairs: result.currencyPairs[item] });
            }
            this.setState({
              isLoaded: true,
              items: me.currencyPairs
            });
          },
          (error) => {
            this.setState({
              isLoaded: true,
              error
            });
          }
        )
    }
  
    render() {
      const { error, isLoaded, items } = this.state;
      if (error) {
        return <div>Error: {error.message}</div>;
      } else if (!isLoaded) {
        return <div>Loading...</div>;
      } else {
        return (
          <ul>
            {items.map(item => (
              <li key={item.id}>
                {item.pairs[0].code}/{item.pairs[1].code}
              </li>
            ))}
          </ul>
        );
      }
    }
  }