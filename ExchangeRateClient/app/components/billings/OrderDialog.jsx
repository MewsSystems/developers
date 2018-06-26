import React from 'react';
import { Dialog } from 'mews-ui';
import { map, uniq, compose, filter, without } from 'lodash/fp';
import styled from 'styled-components'; 
import Product from './Product';


export default class OrderDialog extends React.Component {
    constructor(props) {
        super(props);
    }

    state = {
        products: [
            {
                id: 1,
                name: "supername",
                cost: 400,
            },
            {
                id: 2,
                name: "myproduct",
                cost: 200,
            },
            {
                id: 3,
                name: "mymumsupername",
                cost: 500,
            },
            {
                id: 4,
                name: "notmyproduct",
                cost: 2334300,
            },
        ],
        selectedProductIds: [],
    }

    renderProduct = product => <Product onClick={this.handleProductClick} key={product.id} {...product} />;

    renderProducts = map(this.renderProduct);

    handleProductClick = productId => {
        this.setState({
            selectedProductIds: uniq([...this.state.selectedProductIds, productId]),
        }, () => {
            console.log(this.state);
        });
    }

    handleProductRemove = productId => {
        this.setState({
            selectedProductIds: without([productId], this.state.selectedProductIds),
        }, () => {
            console.log(this.state);
        });
    }

    renderSelectedProduct = product =>(
        <li key={product.id}>
        {product.name}
        <span onClick={() => this.handleProductRemove(product.id)}> X </span>
        </li>
    );

    render() {
        const { products, selectedProductIds } = this.state;
        return (
            <div>
                <Dialog active={true}>
                    <DialogWrapper>
                        <LeftPart>
                            {this.renderProducts(products)}
                        </LeftPart>
                        <RightPart>
                            <ul>
                                {compose(
                                    map(this.renderSelectedProduct),
                                    filter(product => selectedProductIds.indexOf(product.id) !== -1),
                                )(products)}
                            </ul>
                        </RightPart>
                    </DialogWrapper>
                </Dialog>
            </div>
        ); 
    };
};

const DialogWrapper = styled.div`
    display: flex;
`;

const LeftPart = styled.div`
    width: 50%;
    background-color: #f5f5f5;
    height: 316px;
`;

const RightPart = styled.div`
    height: 316px;
    width: 50%;
`;