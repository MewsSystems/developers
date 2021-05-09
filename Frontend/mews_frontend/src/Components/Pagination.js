import React from 'react';
import styled from 'styled-components';

const Nav = styled.nav`
margin-top:3em;
`

const Li = styled.li`
 display:inline-block;

`
const Link = styled.a`
  margin-right: 1em;
  color:#fff;
  padding:0.2em 0.5em;
  background: gray;
  &:hover{
      border-bottom:1px solid #fff;
      background:  #AEB6BF; 
      color:#fff
  }
 `



const Pagination = ({ postsPerPage, totalPosts, paginate }) => {
    const pageNumbers = [];

    for (let i = 1; i <= Math.ceil(totalPosts / postsPerPage); i++) {
        pageNumbers.push(i);
    }

    return (
        <Nav>
            <ul>
                {pageNumbers.map(number => (
                    <Li key={number}>
                        <Link onClick={() => paginate(number)} >
                            {number}
                        </Link>
                    </Li>
                ))}
            </ul>
        </Nav>
    );
};

export default Pagination;