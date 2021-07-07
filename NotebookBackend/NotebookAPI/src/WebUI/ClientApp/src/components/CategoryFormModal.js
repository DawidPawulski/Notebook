import React,{Component} from 'react';
import {Modal,Button,Form} from 'react-bootstrap';
import {create,get, update} from '../helpers/apiHelpers';
import equal from 'fast-deep-equal';

export class CategoryFormModal extends Component{
    constructor(props){
        super(props);
        this.handleSubmit=this.handleSubmit.bind(this);
        this.state={
            categoryName: '',
            categoryid: '',
            category: {},
            updateCategory: false,
        }
    }

    async componentDidUpdate(prevProps, prevState){
        if(!equal(this.props.show, prevProps.show)){
            this.setState({updateCategory: false});
        }

        if(!equal(this.props.categoryid, prevProps.categoryid))
        {
            this.state.categoryid = localStorage.getItem('categoryId');
            var category = await get('categories/' + this.state.categoryid);
            this.setState({category: category});
            this.fillFormWhenCategoryUpdate();
            this.setState({updateCategory: true});
        }
    }
    
    async handleSubmit(event){
        event.preventDefault();
        await this.handleCategoryRequest();
        this.props.onHide();
    }

    async handleCategoryRequest(){
        var request = {
            "name": this.state.categoryName,
        }
        if(this.state.updateCategory){
            await update('categories/'+this.state.categoryid, JSON.stringify(request));
        }
        else{
            await create('categories', JSON.stringify(request));
        }
    }

    fillFormWhenCategoryUpdate(){
        var categoryName = document.getElementById('categoryName');

        if(categoryName != null && this.state.category.name)
        {
            this.setState({categoryName: this.state.category.name});
            categoryName.setAttribute('value', this.state.category.name);
        }
    }

    render(){
        return(
            <div className="container">
                <Modal {...this.props} size="lg" aria-labelledby="contained-modal-title-vcenter" centered>
                <Modal.Header>
                            <Modal.Title id="contained-modal-title-vcenter">
                                {this.state.updateCategory ? 'Update category' : 'Create category'}
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Body class="modal-body">
                                    <Form onSubmit={this.handleSubmit}>
                                        <Form.Group controlId="categoryName">
                                            <Form.Label>Content: </Form.Label>
                                            <Form.Control className="form-input" type="text" name="name" required
                                            onChange = {(event) => this.setState({categoryName: event.target.value })}
                                            placeholder="Category name" />
                                        </Form.Group>

                                        <Form.Group>
                                            <Button varian="primary" type="submit">
                                                {this.state.updateCategory ? 'Update category' : 'Create category'}
                                            </Button>
                                        </Form.Group>
                                    </Form>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="danger" onClick={this.props.onHide}>Close</Button>
                        </Modal.Footer>
                </Modal>
            </div>
        )
    }
}