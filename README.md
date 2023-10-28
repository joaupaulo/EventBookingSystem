# *Aplicação .NET Web Api*
## *Aplicação responsável por simula um sistema de reservas online*
- *Endpoints da aplicação :*
- *Realizar cadastro de usuário para gerar token JWT*
- *Gerar token JWT*
- *Cadastro de eventos*
- *Buscar por todos os eventos disponíveis*
- *Buscar evemtps por chave*
- *Atualizar eventos*
- *Realizar reserva de um determinado evento*
- *Buscar reserva por chave*
- *Buscar todas as reservas disponíveis*
- *Atualizar reservas*
   
  Imagens do swagger:
  ---
  ![image](https://github.com/joaupaulo/EventBookingSystem/assets/61383712/cd8992b3-4ac5-4a3c-8fb6-09d0973126fe)


# *Tecnologias Utilizadas* :
- .Net
- MongoDB
- Docker
- Kubernetes
- Digital Ocean
- Git Actions CI/CD
- Container Registry
- DDD
- Swagger
- Testes Unitários

# Oque foi feito em Devops ? 
Realizamos a configuração do CI/CD com o GitActions, então basicamente quando o Git Actions rodar o workflow configurado, irá ser feito um deploy automatico e seguro na Digital Ocean.

# Como é feito o Deploy na Digital Ocean por baixo dos panos ?
Basicamente publicamos uma imagem dockerizada no Container Registry na Digital Ocean, nisso o Container Registry manda a imagem para o Kubernetes.
