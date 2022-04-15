Funcionalidade: Pedido - Adicionar Item Ao Carrinho
	Como um usuàrio
	Eu desejo colocar um  item ao carrinho
	Para que eu possa comprà-lo posteriormente

Cenario: Adicionar item com sucesso a um novo pedido
Dado Que um produto esteja na vitrine
E Esteja disponivel no estoque
E O usuario esteja logado
Quando o usuario adicionar uma unidade ao carrinho
Então o usuario sera redirecionado ao resumo da compra
E O valor total do pedido sera exatamente o valor do item adicionado

Cenario: Adicionar items acima do limite
Dado Que um produto esteja na vitrine
E Esteja disponivel no estoque
E O usuario esteja logado
Quando o usuario adicionar um item acima da quantidade maxima permitida
Então Recebera uma mensagem de erro mencionado que foi ultrapassada quantidade limite

Cenário: Adicionar item já existente no carrinho
Dado Que um produto esteja na vitrine
E Esteja disponivel no estoque
E O usuario esteja logado
E O mesmo produto já tenha sido adicionado ao carrinho anteriormente
Quando O usuário adicionar uma unidade ao carrinho
Então O usuário será redirecionado ao resumo da compra
E A quantidade de itens daquele produto terá sido acrescida em uma unidade a mais
E O valor total do pedido será a multiplicação da quantidade de itens pelo valor unitario

Cenário: Adicionar item já existente onde soma ultrapassa limite máximo
Dado Que um produto esteja na vitrine
E Esteja disponivel no estoque
E O usuario esteja logado
E O mesmo produto já tenha sido adicionado ao carrinho anteriormente
Quando O usuário adicionar a quantidade máxima permitida ao carrinho
Então O usuário será redirecionado ao resumo da compra
E Receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite

