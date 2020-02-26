import React from 'react';
import { connect } from 'react-redux';

const Pagination = ({ loading, movie: { movies, page, total_pages }, click }) => {

  if (!loading && movies.length !== 0) {
    return (
      <ul className="pagination center">
        <li className={page === 1 ? 'disabled' : 'waves-effect'}>
          <a href="#!" onClick={() => click(page-1)}><i className="material-icons">chevron_left</i></a>
        </li>
        <span>Page: </span><strong>{page} </strong><span>of </span><strong>{total_pages}</strong>
        <li className={page === total_pages ? 'disabled' : 'waves-effect'}>
          <a href="#!" onClick={() => click(page+1)}><i className="material-icons">chevron_right</i></a>
        </li>
      </ul>
    )
  } else {
    return null;
  }
};

const mapStateToProps = state => ({
  movie: state.movie
});

export default connect(mapStateToProps)(Pagination);
