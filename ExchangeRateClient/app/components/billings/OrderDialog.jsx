import React from 'react';
import PropTypes from 'prop-types';
import { Dialog } from 'mews-ui';
import { map, uniq, compose, filter, without } from 'lodash/fp';
import styled from 'styled-components';

import { connect } from 'react-redux';
import {
	fetchConfiguration,
	handleSelectedProducts,
	handleProducts,
} from '../actions/dialogActions';

import Product from './Product';

class OrderDialog extends React.Component {

	componentWillMount() {
		this.props.fetchConfiguration();
	}

	renderProduct = product => <Product onClick={this.onProductClick} key={product.id} {...product} />;

	renderProducts = map(this.renderProduct);

	onProductClick = productId => {
		const selectedProductIds = uniq([...this.props.selectedProductIds, productId]);
		this.props.handleSelectedProducts(selectedProductIds);
	}

	onProductRemove = productId => {
		const selectedProductIds = without([productId], this.props.selectedProductIds);
		this.props.handleSelectedProducts(selectedProductIds);
	}

	degreseProductQuantity = productId => {
		let products = [...this.props.products];
		products.map((product) => {
			if (product.id === productId && product.quantity > 1)
				product.quantity -= 1;
		});
		this.props.handleProducts(products);
	}

	increaseProductQuantity = productId => {
		let products = [...this.props.products];
		products.map((product) => {
			if (product.id === productId && product.quantity < 99)
				product.quantity += 1;
		});
		this.props.handleProducts(products);
	}

	onDialogSubmitHandler = () => {
		let products = [...this.props.products];
		let result = new Array();
		products.map((product) => {
			if (this.props.selectedProductIds.indexOf(product.id) !== -1)
				result[product.id] = product.quantity;
		});
		// result for processing on the server is array in format: productId => count
		console.log(result);
		const s = this.props.selectedProductIds.length > 1 ? `s` : ``;
		alert(
			`Selected product${s} id ${this.props.selectedProductIds} \nSelected product${s} count ${Object.values(result)}`
			);
	};

	// could be another component ProductItem
	renderSelectedProduct = product => (
		<div key={product.id} className={`productItem`}>
			<p>{product.name}
				<i onClick={() => this.onProductRemove(product.id)} className={`fa fa-trash-o`} aria-hidden={true}></i>
			</p>
			<table>
				<thead>
					<tr>
						<th>Amount</th>
						<th>Unit cost</th>
						<th>Total</th>
					</tr>
				</thead>

				<tbody>
					<tr>
						<td>
							<div>
								<i onClick={() => this.degreseProductQuantity(product.id)} className={`fa fa-minus`} aria-hidden={true} />
								<span>{product.quantity}</span>
								<i onClick={() => this.increaseProductQuantity(product.id)} className={`fa fa-plus`} aria-hidden={true} />
							</div>
						</td>
						<td>{`$${product.cost}`}</td>
						<td>{`$${product.cost * product.quantity}`}</td>
					</tr>
				</tbody>
			</table>
		</div>
	);

	render() {
		// const { products, selectedProductIds } = this.props;
		let products = this.props.products;
		let selectedProductIds = this.props.selectedProductIds;
		const closeClasses = `fa fa-close`;
		let finalPrice = null;
		products.filter(product => {
			if (selectedProductIds.indexOf(product.id) !== -1)
				finalPrice += product.cost * product.quantity;
		});
		return (
			<div>
				<Dialog active={true} contentWithoutPadding={true}>
					<Header>
						<OrderName>James Bond order</OrderName>
						<i className={closeClasses} aria-hidden={true}></i>
					</Header>
					<DialogWrapper>
						<LeftPart>
							<Title>Add products</Title>
							{this.renderProducts(products)}
						</LeftPart>
						<RightPart>
							<Title>Order detail</Title>
								{compose(
									map(this.renderSelectedProduct),
									filter(product => selectedProductIds.indexOf(product.id) !== -1),
								)(products)}
								{finalPrice === null ? (
									<Unselected>You don't have any selected product yet.</Unselected>
								) : (
									<div>
										<Total>Order Total: {`$${finalPrice}`}</Total>
										<SubmitButton onClick={this.onDialogSubmitHandler}>Order for {`$${finalPrice}`}</SubmitButton>
									</div>
								)}
						</RightPart>
					</DialogWrapper>
				</Dialog>
			</div>
		);
	};
};

OrderDialog.propTypes = {
	fetchConfiguration: PropTypes.func.isRequired,
	handleSelectedProducts: PropTypes.func.isRequired,
	handleProducts: PropTypes.func.isRequired,
	products: PropTypes.array.isRequired
}

const mapStateToProps = state => ({
	configuration: state.dialogReducer.configuration,
	products: state.dialogReducer.products,
	selectedProductIds: state.dialogReducer.selectedProductIds,
});

export default connect(mapStateToProps, { fetchConfiguration, handleSelectedProducts, handleProducts })(OrderDialog);

// styles could be in some different file
const SubmitButton = styled.button`
	font-family: 'Open Sans', sans-serif;
	display: block;
	margin: 0 auto;
	cursor: pointer;
	text-align: center;
	text-transform: uppercase;
	font-size: 12px;
	background-color: #0091fb;
	color: white;
	padding: 8px 10px;
	border: none;
	border-radius: 2px;
	box-shadow: 0 1px 5px 0 rgba(0,0,0,0.15);
	margin-bottom: 50px;
	margin-top: 20px;
	&:hover {
		background-color: #0383e0;
	}
`;

const Unselected = styled.p`
	text-align: center;
	font-size: 12px;
	opacity: 0.7;
	font-style: italic;
	margin-top: 20px;
`;

const Total = styled.p`
	text-align: right;
	font-weight: 600;
	font-size: 13px;
`;

const Title = styled.h1`
	font-size: 18px;
	font-weight: 400;
	text-align: left;
`;

const OrderName = styled.p`
	font-size: 13px;
	font-weight: bold;
	margin: 0;
	line-height: 50px;
`;

const Header = styled.div`
	display: flex;
	height: 50px;
	padding-left: 23px;
	box-shadow: 0 2px 5px 0 rgba(0,0,0,0.15);
	z-index: 5;
`;

const DialogWrapper = styled.div`
	display: flex;
	height: 400px;
	min-width: 600px;
	padding: 0;
	overflow-y: hidden;
	overflow-x: hidden;
`;

const LeftPart = styled.div`
	width: 50%;
	height: 100%;
	text-align: center;
	padding: 15px 20px;
	background-color: #f5f5f5;
	overflow-y: auto;
`;

const RightPart = styled.div`
	padding: 15px 20px;
	height: 100%;
	width: 50%;
	overflow-y: auto;
`;
