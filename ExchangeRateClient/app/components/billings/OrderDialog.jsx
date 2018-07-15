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
				quantity: 1,
			},
			{
				id: 2,
				name: "myproduct",
				cost: 250,
				quantity: 1,
			},
			{
				id: 3,
				name: "mymumsupernicelongproductname",
				cost: 500,
				quantity: 1,
			},
			{
				id: 4,
				name: "notmyproduct",
				cost: 3250,
				quantity: 1
			},
			{
				id: 5,
				name: "mother-in-law",
				cost: 5,
				quantity: 1
			}
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

	degreseProductQuantity = productId => {
		let products = this.state.products;
		products.map((product) => {
			if (product.id === productId && product.quantity > 1)
				product.quantity -= 1;
		});
		this.setState({ products }, () => {
			console.log(this.state);
		});
	}

	increaseProductQuantity = productId => {
		let products = this.state.products;
		products.map((product) => {
			if (product.id === productId && product.quantity < 99)
				product.quantity += 1;
		});
		this.setState({ products }, () => {
			console.log(this.state);
		});
	}

	onDialogSubmitHandler = () => {
		let products = this.state.products;
		let result = new Array();
		products.map((product) => {
			if (this.state.selectedProductIds.indexOf(product.id) !== -1)
				result[product.id] = product.quantity;
		});
		// result for processing on the server is array in format: productId => count
		console.log(result);
		const s = this.state.selectedProductIds.length > 1 ? `s` : ``;
		alert(
			`Selected product${s} id ${this.state.selectedProductIds} \nSelected product${s} count ${Object.values(result)}`
			);
	};

	renderSelectedProduct = product => (
		<div key={product.id} className={`productItem`}>
			<p>{product.name}
				<i onClick={() => this.handleProductRemove(product.id)} className={`fa fa-trash-o`} aria-hidden={true}></i>
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
		const { products, selectedProductIds } = this.state;
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
