import React from 'react'
import Head from '../components/Head/Head'
import { Col, Container, Row } from '../utils/layout/grid.layout'

const Movie = () => (
    <Container>
        <Head url={''} ogImage={''} title="Movie" description={''} />
        <Row>
            <Col size={12}>
                <div style={{ textAlign: 'center' }}>
                    <h1>About selected movie</h1>
                </div>
            </Col>
        </Row>
    </Container>
)

export default Movie
