import React from 'react';
import PropTypes from 'prop-types';
import { Container, Nav, Navbar as BSNavbar, NavbarBrand } from 'reactstrap';
import SettingsNavItem from './SettingsNavItem';

const Navbar = ({ navbarBrand }) =>
    <BSNavbar color="dark" dark expand="md" className="mb-5">
        <Container>
            <NavbarBrand href="/#/">{navbarBrand}</NavbarBrand>
            <Nav className="ml-auto" navbar>
                <SettingsNavItem />
            </Nav>
        </Container>
    </BSNavbar>;

Navbar.propTypes = {
    navbarBrand: PropTypes.string,
};

export default Navbar;
