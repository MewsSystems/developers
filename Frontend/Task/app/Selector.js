const e = React.createElement

export default class List extends React.Component {
    constructor(props){
        super(props)
        this.state = {
            filter:""
        }
        this.handleChange = this.handleChange.bind(this)
    }

    handleChange(event){
        this.setState({
            filter: event.target.value
        })
        this.props.filterUpdate(event.target.value)
    }

    render() {
        return e('div', {key:'selectorlist'}, [
            e('input', {key: 'inputselector', onChange: this.handleChange})   
        ])
    }
}

